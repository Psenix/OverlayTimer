using Newtonsoft.Json;
using OverlayTimer.Entities;
using OverlayTimer.Global;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MessageBox = ModernWpf.MessageBox;

namespace OverlayTimer
{
    public partial class SubmitPage : Page
    {
        readonly TimeSpan timeScore = TimeSpan.Zero;
        readonly string userName = string.Empty;
        readonly string category = string.Empty;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";

        public SubmitPage(TimeSpan timeScore_, string category_)
        {
            InitializeComponent();
            userName = File.ReadAllText(path + "username");
            category = category_;
            timeScore = timeScore_;
            Time.Text = "Time: " + FormatTime(timeScore);
        }
        private static string FormatTime(TimeSpan time)
        {
            var timeStr = time.ToString("hh'h 'mm'm 'ss's 'fff' ms'");
            while (timeStr.StartsWith("00"))
            {
                timeStr = timeStr.Substring(4);
            }
            return timeStr;
        }

        private void PublicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidURL(VideoLink.Text))
            {
                Entry entry = new Entry
                {
                    TimeScore = timeScore,
                    Username = userName,
                    VideoLink = VideoLink.Text,
                    Category = category,
                    SubmitDate = DateTime.UtcNow,
                };
                Guid result = LeaderboardController.InsertToLeaderboard(entry);
                if (result != Guid.Empty)
                {
                    entry.Guid = result;
                    LocalSubmittion(entry);
                }
                else
                {
                    MessageBox.Show("Could not submit your Speedrun. Request Denied.");
                }
            }
            else
            {
                MessageBox.Show("Please give a valid link to a video of the run, if you want to submit it publicly");
            }
        }

        private void LocalSubmittion(Entry entry)
        {
            GC.Collect();
            string entryStr = JsonConvert.SerializeObject(entry);
            File.AppendAllText(path + category, entryStr + Environment.NewLine);
            GlobalXAML.MainWindow.MainFrame.Navigate(GlobalXAML.MainPage);
            //MessageBox.Show("Succesfully submitted the speedrun", "Success");
        }

        private void LocalBtn_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = new Entry
            {
                TimeScore = timeScore,
                Username = userName,
                VideoLink = VideoLink.Text,
                Category = category,
                SubmitDate = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };
            LocalSubmittion(entry);
        }

        public static bool IsValidURL(string url)
        {
            var period = url.IndexOf(".");
            if (period > -1 && !url.Contains('@'))
            {
                var colon = url.IndexOf(":");
                var slash = url.IndexOf("/");
                if ((colon == -1 || period < colon) &&
                    (slash == -1 || period < slash))
                {
                    url = $"http://{url}";
                }
            }

            var result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }
    }
}
