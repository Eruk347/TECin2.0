using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using TECin2.API.Repositories;
using TECin2.ClassLibrary.Entities;

namespace TECin2.Blazor.Services
{
    public class IpRestrictionMiddleware(RequestDelegate next, IConfiguration config, ILogger<IpRestrictionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly string[] _allowed = config.GetSection("AllowedIPs").Get<string[]>() ?? [];
        private readonly ILogger<IpRestrictionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = GetRemoteIp(context);
            var target = context.Request.Path;

            var loggerRepository = context.RequestServices.GetRequiredService<LoggerRepository>();

            if (remoteIp.ToString() != "127.0.0.1")
            {
                Log log = new()
                {
                    DateAndTime = DateTime.Now,
                    Message = "" + remoteIp.ToString() + " target: " + target.ToString() + " full request: " + context.Request.GetDisplayUrl(),
                    User = "IPRestrictionMIddleware"
                };

                await loggerRepository.WriteLog(log);
            }

            if (remoteIp == null)
            {
                _logger.LogWarning("Could not determine remote IP - denying request.");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            var allowed = _allowed.Any(entry =>
            {
                if (string.IsNullOrWhiteSpace(entry)) return false;
                entry = entry.Trim();
                if (entry.Contains('/'))
                {
                    return IsInCidr(remoteIp, entry);
                }
                if (IPAddress.TryParse(entry, out var allowedIp))
                {
                    return allowedIp.Equals(remoteIp);
                }
                return false;
            });
            LoggerRepository.WriteLog("" + DateTime.Now + ": IPRestrictionMiddleware: Request from IP " + remoteIp.ToString() + " targeting " + target.ToString() + " allowed: " + allowed);
            if (!allowed)
            {
                Log logBlock = new()
                {
                    DateAndTime = DateTime.Now,
                    Message = "Blocked request from IP " + remoteIp.ToString() + ", targeting: " + target.ToString(),
                    User = "Block"
                };
                await loggerRepository.WriteLog(logBlock);
                LoggerRepository.WriteLog("Blocked request from IP " + remoteIp.ToString() + ", targeting: " + target.ToString());
                _logger.LogInformation("Blocked request from IP {RemoteIp}", remoteIp);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }

        private IPAddress GetRemoteIp(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var header) && header.Count > 0)
            {
                var first = header.ToString().Split(',').Select(s => s.Trim()).FirstOrDefault();
                if (IPAddress.TryParse(first, out var ipFromHeader))
                    return ipFromHeader;
            }
            return context.Connection.RemoteIpAddress;
        }

        private bool IsInCidr(IPAddress address, string cidr)
        {
            if (address == null) return false;
            var parts = cidr.Split('/');
            if (parts.Length != 2) return false;
            if (!IPAddress.TryParse(parts[0], out var network)) return false;
            if (!int.TryParse(parts[1], out var prefix)) return false;

            var addrBytes = address.MapToIPv4().GetAddressBytes();
            var netBytes = network.MapToIPv4().GetAddressBytes();
            if (addrBytes.Length != 4 || netBytes.Length != 4) return false;

            uint addr = BitConverter.ToUInt32(addrBytes.Reverse().ToArray(), 0);
            uint net = BitConverter.ToUInt32(netBytes.Reverse().ToArray(), 0);
            uint mask = prefix == 0 ? 0u : uint.MaxValue << (32 - prefix);
            return (addr & mask) == (net & mask);
        }
    }
}
