using System.Net;
using Microsoft.AspNetCore.Http;

namespace Firewall
{
    /// <summary>
    /// A Firewall rule which permits access to connections from localhost.
    /// </summary>
    public sealed class LocalhostRule : IFirewallRule
    {
        private readonly IFirewallRule _nextRule;

        /// <summary>
        /// Initialises a new instance of <see cref="LocalhostRule"/>.
        /// </summary>
        public LocalhostRule(IFirewallRule nextRule)
        {
            _nextRule = nextRule;
        }

        /// <summary>
        /// Denotes whether a given <see cref="HttpContext"/> is permitted to access the web server.
        /// </summary>
        public bool IsAllowed(HttpContext context)
        {
            var localIpAddress = context.Connection.LocalIpAddress;
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            var isAllowed =
                (remoteIpAddress != null
                    && remoteIpAddress.ToString() != "::1"
                    && remoteIpAddress.Equals(localIpAddress))
                || IPAddress.IsLoopback(remoteIpAddress);

            return isAllowed || _nextRule.IsAllowed(context);
        }
    }
}