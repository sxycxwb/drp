﻿using System;
using System.Web;

namespace StackExchange.Profiling
{
    /// <summary>
    /// Identifies users based on ip address.
    /// </summary>
    public class IpAddressIdentity : IUserProvider
    {
        /// <summary>
        /// Returns the paramter HttpRequest's client ip address.
        /// We combine both the REMOTE_ADDR header (which is the connecting device's IP address),
        /// plus the HTTP_X_FORWARDED_FOR header if present (which is set by some proxy
        /// servers and load balancers). This allows us to have a unique per-user view, even
        /// when behind a proxy or load balancer.
        /// </summary>
        public string GetUser(HttpRequest request)
        {
            // If there's no X_FORWARDED_FOR header, just return REMOTE_ADDR
            if (String.IsNullOrWhiteSpace(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                return request.ServerVariables["REMOTE_ADDR"] ?? "";
            }

            // Otherwise return the concatenation of the REMOTE_ADDR and the X_FORWARDED_FOR header
            return string.Format("{0} - {1}", request.ServerVariables["REMOTE_ADDR"] ?? "",
                                              request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
        }
    }
}
