using Newtonsoft.Json;
using OverlayTimer.Controllers;
using OverlayTimer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OverlayTimer
{
    public partial class SubmitPage : Page
    {
        readonly TimeSpan timeScore = TimeSpan.Zero;
        readonly string userName = string.Empty;
        readonly string category = string.Empty;
        readonly MainPage mainPage;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        MainWindow mainWindow;

        public SubmitPage(TimeSpan timeScore_, string userName_, string category_, MainWindow mainWindow_, MainPage mainPage_)
        {
            InitializeComponent();
            timeScore = timeScore_;
            userName = userName_;
            category = category_;
            mainWindow = mainWindow_;
            mainPage = mainPage_;
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
                };
                LeaderboardController.InsertToLeaderboard(entry);
                LocalSubmittion(entry);
            }
            else
            {
                MessageBox.Show("Please give a valid link to a video of the run, if you want to submit it publicly");
            }
        }

        private void LocalSubmittion(Entry entry)
        {
            string entryStr = JsonConvert.SerializeObject(entry);
            File.AppendAllText(path + category, entryStr);
            mainWindow.MainFrame.Navigate(mainPage);
            MessageBox.Show("Succesfully submitted");
        }

        private void LocalBtn_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = new Entry
            {
                TimeScore = timeScore,
                Username = userName,
                VideoLink = VideoLink.Text,
                Category = category,
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
    }
}
