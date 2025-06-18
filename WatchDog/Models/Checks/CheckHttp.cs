using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class CheckHttp : CheckBase
    {
        public CheckHttp() 
        {
            GUID = "<Guid(\"CEFF62E6-08B1-4509-9702-7641ADB3A148\")>";
            Name = "HTTP request";
        }

        public override async Task<CheckResult> CheckAsync(string url)
        {

            var result = new CheckResult
            {
                MethodName = Name,
                Address = url,
                IsAlive = false,
                ResponseTime = string.Empty,
                ExtraMessage = string.Empty
            };


            if (!url.StartsWith("http://") && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                    
                    DateTime startTime = DateTime.Now; 
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    DateTime endTime = DateTime.Now; 

                    var responseTime = endTime - startTime;
                    result.ResponseTime = responseTime.TotalMilliseconds + " ms";

                    result.IsAlive = response.IsSuccessStatusCode; 
                    result.ExtraMessage = response.IsSuccessStatusCode
                        ? $"Response Status Code: {response.StatusCode}"
                        : $"Failed with Status Code: {response.StatusCode}";
                }
                catch (Exception ex)
                {
                    result.ExtraMessage = $"Exception: {ex.Message}";
                }

                return result;
            }
        }

    }
}
