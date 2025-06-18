using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class NotifyBot
    {
        private HttpClient Client { get; set; }
        public string Token { get; set; }
        public string ChatId { get; set; }

        public NotifyBot() {
            Client = new HttpClient();
        }
        public async Task SendMessageAsync(string message)
        {
            string url = $"https://api.telegram.org/bot{Token}/sendMessage?chat_id={ChatId}&text={Uri.EscapeDataString(message)}";
        
            HttpResponseMessage response = await Client.GetAsync(url);
        }

    }
}
