using Newtonsoft.Json;
using OverlayTimer.Controllers;
using OverlayTimer.Entities;
using OverlayTimer.Global;
using OverlayTimer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using static OverlayTimer.Models.DataModel;
using MessageBox = ModernWpf.MessageBox;

namespace OverlayTimer.Pages
{
    public partial class LeaderboardPage : Page
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        readonly DataModel dataModel = new DataModel();

        public LeaderboardPage()
        {
            InitializeComponent();
            LeaderboardList.ItemsSource = dataModel.Data;
            Task task = new Task(GetAvailableGames);
            task.Start();
        }

        private void GetAvailableGames()
        {
            List<string> games = GamesController.GetGames();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Games.Items.RemoveAt(0);
                foreach (var game in games)
                {
                    Games.Items.Add(new ComboBoxItem() { Content = game });
                }
            }));
        }

        private void Games_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
                LoadLeaderboard(LocalPublic.Text, selectedItem);
            }
        }

        private void LocalPublic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            LoadLeaderboard(selectedItem, Games.Text);
        }

        private void LoadLeaderboard(string publicOrLocal, string game)
        {
            try
            {
                dataModel.Data.Clear();
                if (publicOrLocal == "Public")
                {
                    GetPublicLeaderboard(game);
                }
                else if (publicOrLocal == "Local")
                {
                    GetLocalLeaderboard(game);
                }
                LeaderboardList.Items.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void GetPublicLeaderboard(string selectedItem)
        {
            var list = LeaderboardController.GetLeaderboard(selectedItem);
            list.Sort((x, y) => TimeSpan.Compare(x.TimeScore, y.TimeScore));
            foreach (var item in list)
            {
                string previewLink = FormatURL(item.VideoLink);
                string time = FormatTime(item.TimeScore);
                DataEntity entity = new DataEntity(item.Username, time, item.SubmitDate.ToLocalTime().ToShortDateString(), previewLink, item.VideoLink);
                dataModel.Data.Add(entity);
                LeaderboardList.Items.Refresh();
            }
        }

        private void GetLocalLeaderboard(string selectedItem)
        {
            if (File.Exists(path + selectedItem))
            {
                var stringArray = File.ReadAllLines(path + selectedItem);
                List<Entry> entries = new List<Entry>();
                foreach (var item in stringArray)
                {
                    entries.Add(JsonConvert.DeserializeObject<Entry>(item));
                }
                entries.Sort((x, y) => TimeSpan.Compare(x.TimeScore, y.TimeScore));
                foreach (var item in entries)
                {
                    string previewLink = FormatURL(item.VideoLink);
                    string time = FormatTime(item.TimeScore);
                    DataEntity entity = new DataEntity(item.Username, time, item.SubmitDate.ToLocalTime().ToShortDateString(), previewLink, item.VideoLink);
                    dataModel.Data.Add(entity);
                    LeaderboardList.Items.Refresh();
                }
            }
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

        private static string FormatURL(string previewLink)
        {
            if (!string.IsNullOrWhiteSpace(previewLink))
            {
                if (previewLink.StartsWith("http://"))
                {
                    previewLink = previewLink.Remove(0, 7);
                }
                else if (previewLink.StartsWith("https://"))
                {
                    previewLink = previewLink.Remove(0, 8);
                }
                //if (previewLink.Length > 30)
                //{
                //    previewLink = previewLink.Substring(0, 30);
                //    previewLink += "...";
                //}
            }
            else
            {
                return "Not specified";
            }
            return previewLink;
        }

        private void LeaderboardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
            {
                var item = (DataEntity)e.AddedItems[0];
                if (!string.IsNullOrWhiteSpace(item.VideoLink))
                {
                    Process.Start(item.VideoLink);
                }
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Games.SelectedItem = null;
            Games.Text = "Select a game";
            dataModel.Data.Clear();
            LeaderboardList.Items.Refresh();
            LocalPublic.Text = "Public";
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineDown();
            }
            else
            {
                scrollViewer.LineUp();
            }
            e.Handled = true;
        }
    }
}
