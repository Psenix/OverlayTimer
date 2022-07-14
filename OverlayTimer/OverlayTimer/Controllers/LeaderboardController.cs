using Newtonsoft.Json;
using OverlayTimer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OverlayTimer
{
    internal class LeaderboardController
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://overlaytimerapi.azurewebsites.net/"),
            //BaseAddress = new Uri("https://localhost:7001/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static bool InsertToLeaderboard(Entry entry)
        {
            try
            {
                HttpContent stringContent = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
                var response = client.PostAsync("Leaderboard/InsertToLeaderboard", stringContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                if (content != "true")
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<Entry> GetLeaderboard(string game)
        {
            try
            {
                var response = client.GetAsync("Leaderboard/GetLeaderboard?game=" + game).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                List<Entry> entries = JsonConvert.DeserializeObject<List<Entry>>(content);
                return entries;
            }
            catch
            {
                return null;
            }
        }
    }
}
