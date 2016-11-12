﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace StackExchange.Profiling.Helpers
{
    /// <summary>
    ///     Gets part of a stack trace containing only methods we care about.
    /// </summary>
    public class StackTraceSnippet
    {
        private const string AspNetEntryPointMethodName = "System.Web.HttpApplication.IExecutionStep.Execute";

        /// <summary>
        ///     Gets the current formatted and filtered stack trace.
        /// </summary>
        /// <returns>Space separated list of methods</returns>
        public static string Get()
        {
            var frames = new StackTrace().GetFrames();
            if (frames == null || MiniProfiler.Settings.StackMaxLength <= 0)
            {
                return "";
            }

            var methods = new List<string>();
            int stackLength = -1; // Starts on -1 instead of zero to compensate for adding 1 first time

            foreach (StackFrame t in frames)
            {
                var method = t.GetMethod();

                // no need to continue up the chain
                if (method.Name == AspNetEntryPointMethodName)
                    break;

                if (stackLength >= MiniProfiler.Settings.StackMaxLength)
                    break;

                var assembly = method.Module.Assembly.GetName().Name;
                if (!ShouldExcludeType(method)
                    && !MiniProfiler.Settings.AssembliesToExclude.Contains(assembly)
                    && !MiniProfiler.Settings.MethodsToExclude.Contains(method.Name))
                {
                    methods.Add(method.Name);
                    stackLength += method.Name.Length + 1; // 1 added for spaces.
                }
            }

            return string.Join(" ", methods);
        }

        private static bool ShouldExcludeType(MethodBase method)
        {
            var t = method.DeclaringType;

            while (t != null)
            {
                if (MiniProfiler.Settings.TypesToExclude.Contains(t.Name))
                    return true;

                t = t.DeclaringType;
            }

            return false;
        }
    }
}