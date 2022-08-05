using Newtonsoft.Json;
using OverlayTimer.Entities;
using OverlayTimer.Global;
using System;
using System.IO;
using System.Linq;
using System.Threading;
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
            GlobalXAML.MainWindow.Activate();
            GlobalXAML.MainWindow.Topmost = true;
            GlobalXAML.MainWindow.Topmost = false;
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
                if (!LeaderboardController.IsLinkDuplicate(VideoLink.Text))
                {
                    PublicBtn.IsEnabled = false;
                    Thread thread = new Thread(UploadEntry);
                    thread.Start();
                }
                else
                {
                    MessageBox.Show("The entered video link is already uploaded. Please use a different video.");
                }
            }
            else
            {
                MessageBox.Show("Please give a valid link to a video of the run, if you want to submit it publicly");
            }
        }

        private void UploadEntry()
        {
            Entry entry = new Entry
            {
                TimeScore = timeScore,
                Username = userName,
                Category = category,
                SubmitDate = DateTime.UtcNow,
                Key = Guid.Parse(File.ReadAllText(path + "key"))
            };
            Dispatcher.Invoke(new Action(() => entry.VideoLink = VideoLink.Text));
            if (!LeaderboardController.IsLinkDuplicate(entry.VideoLink))
            {
                Guid result = LeaderboardController.InsertToLeaderboard(entry);
                if (result != Guid.Empty)
                {
                    entry.Guid = result;
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Your speedrun will be displayed publicly as soon as a moderator approves it.", "Successfully submitted");
                        LocalSubmittion(entry);
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("Could not submit your Speedrun. Request Denied.")));
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("There is already a speedrun that has your video link. Please enter another one.")));
            }
            Dispatcher.BeginInvoke(new Action(() => PublicBtn.IsEnabled = true));
        }

        private void LocalSubmittion(Entry entry)
        {
            GC.Collect();
            string entryStr = JsonConvert.SerializeObject(entry);
            File.AppendAllText(path + category, entryStr + Environment.NewLine);
            GlobalXAML.MainWindow.MainFrame.Navigate(GlobalXAML.MainPage);
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
