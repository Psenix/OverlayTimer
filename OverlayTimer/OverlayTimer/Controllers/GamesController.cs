using Newtonsoft.Json;
using OverlayTimer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OverlayTimer.Controllers
{
    internal class GamesController
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://overlaytimerapi.azurewebsites.net/"),
            //BaseAddress = new Uri("https://localhost:7001/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static List<string> GetGames()
        {
            try
            {
                var response = client.GetAsync("Leaderboard/GetSupportedGames").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                List<string> entries = JsonConvert.DeserializeObject<List<string>>(content);
                return entries;
            }
            catch
            {
                return null;
            }
        }
    }
}
