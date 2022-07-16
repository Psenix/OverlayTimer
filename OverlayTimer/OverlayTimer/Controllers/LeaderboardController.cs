using Newtonsoft.Json;
using OverlayTimer.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        public static Guid InsertToLeaderboard(Entry entry)
        {
            try
            {
                HttpContent stringContent = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
                var response = client.PostAsync("Leaderboard/InsertToLeaderboard", stringContent).Result;
                var guid = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
                return guid;
            }
            catch
            {
                return Guid.Empty;
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
