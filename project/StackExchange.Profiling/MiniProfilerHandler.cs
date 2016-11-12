﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using StackExchange.Profiling.Helpers;

namespace StackExchange.Profiling
{
    /// <summary>
    /// Understands how to route and respond to MiniProfiler UI URLS.
    /// </summary>
    public class MiniProfilerHandler : IRouteHandler, IHttpHandler
    {
        /// <summary>
        /// Embedded resource contents keyed by filename.
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> ResourceCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Gets a value indicating whether to keep things static and reusable.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Usually called internally, sometimes you may clear the routes during the apps lifecycle, 
        /// if you do that call this to bring back mini profiler.
        /// </summary>
        public static void RegisterRoutes()
        {
            var routes = RouteTable.Routes;
            var handler = new MiniProfilerHandler();
            var prefix = MiniProfiler.Settings.RouteBasePath.Replace("~/", string.Empty).EnsureTrailingSlash();

            using (routes.GetWriteLock())
            {
                var route = new Route(prefix + "{filename}", handler)
                {
                    // specify these, so no MVC route helpers will match, e.g. @Html.ActionLink("Home", "Index", "Home")
                    Defaults = new RouteValueDictionary(new { controller = "MiniProfilerHandler", action = "ProcessRequest" }),
                    Constraints = new RouteValueDictionary(new { controller = "MiniProfilerHandler", action = "ProcessRequest" })
                };

                // put our routes at the beginning, like a boss
                routes.Insert(0, route);
            }
        }

        /// <summary>
        /// Returns this <see cref="MiniProfilerHandler"/> to handle <paramref name="requestContext"/>.
        /// </summary>
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        /// <summary>
        /// Returns either includes' <c>css/javascript</c> or results' html.
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            string output;
            string path = context.Request.AppRelativeCurrentExecutionFilePath;

            switch (Path.GetFileNameWithoutExtension(path).ToLowerInvariant())
            {
                case "underscore":
                case "jquery.1.7.1":
                case "jquery.tmpl":
                case "list":
                    output = Includes(context, path);
                    break;
                case "includes":
                    output = Includes(context, path);
                    break;

                case "results-index":
                    output = Index(context);
                    break;

                case "results-list":
                    output = GetListJson(context);
                    break;

                case "results":
                    output = GetSingleProfilerResult(context);
                    break;

                default:
                    output = NotFound(context);
                    break;
            }

            if (MiniProfiler.Settings.EnableCompression && !string.IsNullOrEmpty(output))
            {
                Compression.EncodeStreamAndAppendResponseHeaders(context.Request, context.Response);
            }

            context.Response.Write(output);
        }

        /// <summary>
        /// Renders script tag found in "include.partial.html".
        /// </summary>
        internal static HtmlString RenderIncludes(
            MiniProfiler profiler,
            RenderPosition? position = null,
            bool? showTrivial = null,
            bool? showTimeWithChildren = null,
            int? maxTracesToShow = null,
            bool? showControls = null,
            bool? startHidden = null)
        {
            if (profiler == null) return new HtmlString("");

            MiniProfiler.Settings.EnsureStorageStrategy();
            var authorized = MiniProfiler.Settings.Results_Authorize == null
                || MiniProfiler.Settings.Results_Authorize(HttpContext.Current.Request);

            // unviewed ids are added to this list during Storage.Save, but we know we haven't 
            // seen the current one yet, so go ahead and add it to the end 
            var ids = authorized ? MiniProfiler.Settings.Storage.GetUnviewedIds(profiler.User) : new List<Guid>();
            ids.Add(profiler.Id);

            string format;
            if (!TryGetResource("include.partial.html", out format))
            {
                return (new HtmlString("<!-- Could not find 'include.partial.html' -->"));
            }

            return new HtmlString(format.Format(new
            {
                path = VirtualPathUtility.ToAbsolute(MiniProfiler.Settings.RouteBasePath).EnsureTrailingSlash(),
                version = MiniProfiler.Settings.Version,
                currentId = profiler.Id,
                ids = string.Join(",", ids.Select(guid => guid.ToString())),
                position = (position ?? MiniProfiler.Settings.PopupRenderPosition).ToString().ToLower(),
                showTrivial = (showTrivial ?? MiniProfiler.Settings.PopupShowTrivial).ToJs(),
                showChildren = (showTimeWithChildren ?? MiniProfiler.Settings.PopupShowTimeWithChildren).ToJs(),
                maxTracesToShow = maxTracesToShow ?? MiniProfiler.Settings.PopupMaxTracesToShow,
                showControls = (showControls ?? MiniProfiler.Settings.ShowControls).ToJs(),
                authorized = authorized.ToJs(),
                toggleShortcut = MiniProfiler.Settings.PopupToggleKeyboardShortcut,
                startHidden = (startHidden ?? MiniProfiler.Settings.PopupStartHidden).ToJs(),
                trivialMilliseconds = MiniProfiler.Settings.TrivialDurationThresholdMilliseconds
            }));
        }

        /// <summary>
        /// Handles rendering static content files.
        /// </summary>
        private static string Includes(HttpContext context, string path)
        {
            var response = context.Response;
            switch (Path.GetExtension(path))
            {
                case ".js":
                    response.ContentType = "application/javascript";
                    break;
                case ".css":
                    response.ContentType = "text/css";
                    break;
                case ".tmpl":
                    response.ContentType = "text/x-jquery-tmpl";
                    break;
                default:
                    return NotFound(context);
            }
#if !DEBUG
            var cache = response.Cache;
            cache.SetCacheability(System.Web.HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.AddDays(7));
            cache.SetValidUntilExpires(true);
#endif
            string resource;
            return TryGetResource(Path.GetFileName(path), out resource) ? resource : NotFound(context);
        }

        private static string Index(HttpContext context)
        {
            string message;
            if (!AuthorizeRequest(context, isList: true, message: out message))
            {
                return message;
            }

            context.Response.ContentType = "text/html";

            var path = VirtualPathUtility.ToAbsolute(MiniProfiler.Settings.RouteBasePath).EnsureTrailingSlash();
            return new StringBuilder()
                .AppendLine("<html><head>")
                .AppendLine("<title>List of profiling sessions</title>")
                .AppendFormat("<script id='mini-profiler' data-ids='' type='text/javascript' src='{0}includes.js?v={1}'></script>{2}", path, MiniProfiler.Settings.Version, Environment.NewLine)
                .AppendFormat("<link href='{0}includes.css?v={1}' rel='stylesheet' type='text/css'>{2}", path, MiniProfiler.Settings.Version, Environment.NewLine)
                .AppendFormat("<script type='text/javascript' src='{0}includes.js?v={1}'></script>{2}", path, MiniProfiler.Settings.Version, Environment.NewLine)
                .AppendFormat("<script type='text/javascript'>MiniProfiler.list.init({{path: '{0}', version: '{1}'}})</script>{2}", path, MiniProfiler.Settings.Version, Environment.NewLine)
                .AppendLine("</head></html>")
                .ToString();
        }

        /// <summary>
        /// Returns true if the current request is allowed to see the profiler response.
        /// </summary>
        private static bool AuthorizeRequest(HttpContext context, bool isList, out string message)
        {
            message = null;
            var authorize = MiniProfiler.Settings.Results_Authorize;
            var authorizeList = MiniProfiler.Settings.Results_List_Authorize;

            if ((authorize != null && !authorize(context.Request)) || (isList && (authorizeList == null || !authorizeList(context.Request))))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                message = "unauthorized";
                return false;
            }

            return true;
        }

        private static string GetListJson(HttpContext context)
        {
            string message;
            if (!AuthorizeRequest(context, isList: true, message: out message))
            {
                return message;
            }

            var lastId = context.Request["last-id"];
            Guid lastGuid = Guid.Empty;

            if (!lastId.IsNullOrWhiteSpace())
            {
                Guid.TryParse(lastId, out lastGuid);
            }

            // After app restart, MiniProfiler.Settings.Storage will be null if no results saved, and NullReferenceException is thrown.
            if (MiniProfiler.Settings.Storage == null)
            {
                MiniProfiler.Settings.EnsureStorageStrategy();
            }

            var guids = MiniProfiler.Settings.Storage.List(100);

            if (lastGuid != Guid.Empty)
            {
                guids = guids.TakeWhile(g => g != lastGuid);
            }

            guids = guids.Reverse();

            return guids.Select(
                g =>
                {
                    var profiler = MiniProfiler.Settings.Storage.Load(g);
                    
                    if (profiler == null)
                    {
                        return null;
                    }
                    
                    return new
                    {
                        profiler.Id,
                        profiler.Name,
                        profiler.ClientTimings,
                        profiler.Started,
                        profiler.HasUserViewed,
                        profiler.MachineName,
                        profiler.User,
                        profiler.DurationMilliseconds
                    };
                }).Where(x => x != null).ToJson();
        }

        /// <summary>
        /// Returns either json or full page html of a previous <c>MiniProfiler</c> session, 
        /// identified by its <c>"?id=GUID"</c> on the query.
        /// </summary>
        private static string GetSingleProfilerResult(HttpContext context)
        {
            // when we're rendering as a button/popup in the corner, we'll pass ?popup=1
            // if it's absent, we're rendering results as a full page for sharing
            var isPopup = context.Request["popup"].HasValue();

            // this guid is the MiniProfiler.Id property
            // if this guid is not supplied, the last set of results needs to be
            // displayed. The home page doesn't have profiling otherwise.
            Guid id;
            if (!Guid.TryParse(context.Request["id"], out id) && MiniProfiler.Settings.Storage != null)
                id = MiniProfiler.Settings.Storage.List(1).FirstOrDefault();

            if (id == default(Guid))
                return isPopup ? NotFound(context) : NotFound(context, "text/plain", "No Guid id specified on the query string");

            MiniProfiler.Settings.EnsureStorageStrategy();
            var profiler = MiniProfiler.Settings.Storage.Load(id);

            var provider = WebRequestProfilerProvider.Settings.UserProvider;
            string user = null;
            if (provider != null)
            {
                user = provider.GetUser(context.Request);
            }

            MiniProfiler.Settings.Storage.SetViewed(user, id);

            if (profiler == null)
            {
                return isPopup ? NotFound(context) : NotFound(context, "text/plain", "No MiniProfiler results found with Id=" + id.ToString());
            }

            bool needsSave = false;
            if (profiler.ClientTimings == null)
            {
                profiler.ClientTimings = ClientTimings.FromRequest(context.Request);
                if (profiler.ClientTimings != null)
                {
                    needsSave = true;
                }
            }

            if (!profiler.HasUserViewed)
            {
                profiler.HasUserViewed = true;
                needsSave = true;
            }

            if (needsSave)
            {
                MiniProfiler.Settings.Storage.Save(profiler);
            }

            string authorizeMessage;
            if (!AuthorizeRequest(context, isList: false, message: out authorizeMessage))
            {
                context.Response.ContentType = "application/json";
                return "hidden".ToJson();
            }

            return isPopup ? ResultsJson(context, profiler) : ResultsFullPage(context, profiler);
        }

        private static string ResultsJson(HttpContext context, MiniProfiler profiler)
        {
            context.Response.ContentType = "application/json";
            return MiniProfiler.ToJson(profiler);
        }

        private static string ResultsFullPage(HttpContext context, MiniProfiler profiler)
        {
            context.Response.ContentType = "text/html";

            string template;
            if (!TryGetResource("share.html", out template))
                return NotFound(context);
            return template.Format(new
            {
                name = profiler.Name,
                duration = profiler.DurationMilliseconds.ToString(CultureInfo.InvariantCulture),
                path = VirtualPathUtility.ToAbsolute(MiniProfiler.Settings.RouteBasePath).EnsureTrailingSlash(),
                json = MiniProfiler.ToJson(profiler),
                includes = RenderIncludes(profiler),
                version = MiniProfiler.Settings.Version
            });
        }

        private static bool TryGetResource(string filename, out string resource)
        {
            filename = filename.ToLower();

#if DEBUG
            // attempt to simply load from file system, this lets up modify js without needing to recompile A MILLION TIMES 
            if (!BypassLocalLoad)
            {

                var trace = new System.Diagnostics.StackTrace(true);
                var path = Path.GetDirectoryName(trace.GetFrames()[0].GetFileName()) + "\\ui\\" + filename;
                try
                {
                    resource = File.ReadAllText(path);
                    return true;
                }
                catch
                {
                    BypassLocalLoad = true;
                }
            }
#endif

            if (!ResourceCache.TryGetValue(filename, out resource))
            {
                string customTemplatesPath = HttpContext.Current.Server.MapPath(MiniProfiler.Settings.CustomUITemplates);
                string customTemplateFile = Path.Combine(customTemplatesPath, filename);

                if (File.Exists(customTemplateFile))
                {
                    resource = File.ReadAllText(customTemplateFile);
                }
                else
                {
                    using (var stream = typeof(MiniProfilerHandler).Assembly.GetManifestResourceStream("StackExchange.Profiling.ui." + filename))
                    {
                        if (stream == null)
                        {
                            return false;
                        }
                        using (var reader = new StreamReader(stream))
                        {
                            resource = reader.ReadToEnd();
                        }
                    }
                }

                ResourceCache[filename] = resource;
            }

            return true;
        }

#if DEBUG
        private static bool BypassLocalLoad = false;
#endif

        private static string NotFound(HttpContext context, string contentType = "text/plain", string message = null)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = contentType;

            return message;
        }

    }
}
