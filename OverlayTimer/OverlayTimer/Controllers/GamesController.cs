using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OverlayTimer.Controllers
{
    internal class GamesController
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://overlaytimerapi.azurewebsites.net/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static Dictionary<string, string> GetGames()
        {
            try
            {
                var response = client.GetAsync("Leaderboard/GetSupportedGames").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, string> entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                return entries;
            }
            catch
            {
                return null;
            }
        }
    }
}
