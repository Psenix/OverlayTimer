using Newtonsoft.Json;
using OverlayTimer.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace OverlayTimer
{
    internal class LeaderboardController
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://overlaytimerapi.azurewebsites.net/"),
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

        public static Guid GetGuidFromID(string ID, string password)
        {
            try
            {
                var response = client.GetAsync("Leaderboard/GetGuidFromID?ID=" + ID + "&password=" + password).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var guid = JsonConvert.DeserializeObject<Guid>(content);
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

        public static List<Entry> GetUnverifiedEntries(string token)
        {
            try
            {
                var response = client.GetAsync("Leaderboard/GetUnverifiedEntries?password=" + token).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                List<Entry> entries = JsonConvert.DeserializeObject<List<Entry>>(content);
                return entries;
            }
            catch
            {
                return null;
            }

        }

        public static bool VerifyEntry(Entry entry, string token)
        {
            try
            {
                HttpContent stringContent = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
                var response = client.PostAsync("Leaderboard/VerifyEntry?password=" + token, stringContent).Result;
                return JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteEntry(string guid, string username)
        {
            try
            {
                HttpContent stringContent = new StringContent("");
                var response = client.PostAsync("Leaderboard/DeleteEntry?guid=" + guid + "&username=" + username, stringContent).Result;
                return JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidToken(string token)
        {
            try
            {
                var response = client.GetAsync("Leaderboard/CorrectPassword?password=" + token).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(content);
            }
            catch
            {
                return false;
            }
        }
    }
}
