using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class CheckPing : CheckBase
    {
        public CheckPing()
        {
            GUID = "<Guid(\"8826141D-1400-4505-98C9-BC3B7D64EB10\")>";
            Name = "Ping request";
        }

        public override async Task<CheckResult> CheckAsync(string url)
        {
            var result = new CheckResult
            {
                MethodName = "Ping",
                Address = url,
                IsAlive = false,
                ResponseTime = string.Empty,
                ExtraMessage = string.Empty
            };

            try
            {
                var uri = new Uri("http://"+url);
                string host = uri.Host;

                using (var ping = new Ping())
                {
                    DateTime startTime = DateTime.Now; 
                    PingReply reply = await ping.SendPingAsync(host);
                    DateTime endTime = DateTime.Now;

                    var responseTime = endTime - startTime;
                    result.ResponseTime = responseTime.TotalMilliseconds + " ms";

                    if (reply.Status == IPStatus.Success)
                    {
                        result.IsAlive = true;
                        result.ExtraMessage = "Ping successful.";
                    }
                    else
                    {
                        result.ExtraMessage = $"Ping failed: {reply.Status}.";
                    }
                }
            }
            catch (PingException pingEx)
            {
                result.ExtraMessage = $"Ping exception: {pingEx.Message}";
            }
            catch (Exception ex)
            {
                result.ExtraMessage = $"Exception: {ex.Message}";
            }

            return result;
        }
    }
}
