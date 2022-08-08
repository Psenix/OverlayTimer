using Newtonsoft.Json;
using OverlayTimer.Controllers;
using OverlayTimer.Entities;
using OverlayTimer.Models;
using OverlayTimer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static OverlayTimer.Models.DataModel;
using MessageBox = ModernWpf.MessageBox;

namespace OverlayTimer.Pages
{
    public partial class LeaderboardPage : Page
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\LocalLeaderboard\\";
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
            Dictionary<string, string> games = GamesController.GetGames();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    Games.Items.RemoveAt(0);
                    foreach (var game in games)
                    {
                        Games.Items.Add(new ComboBoxItem() { Content = game.Key });
                    }
                }
                catch
                {
                    Games.Items.Add(new ComboBoxItem() { Content = "Could not fetch data", IsEnabled = false });
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
            if (list.Count > 0)
            {
                NoRuns.Visibility = Visibility.Collapsed;
                list.Sort((x, y) => TimeSpan.Compare(x.TimeScore, y.TimeScore));
                foreach (var item in list)
                {
                    DataEntity entity = new DataEntity(item);
                    dataModel.Data.Add(entity);
                    LeaderboardList.Items.Refresh();
                }
            }
            else
            {
                NoRuns.Visibility = Visibility.Visible;
            }
        }

        private void GetLocalLeaderboard(string selectedItem)
        {
            if (File.Exists(path + selectedItem))
            {
                NoRuns.Visibility = Visibility.Collapsed;
                var stringArray = File.ReadAllLines(path + selectedItem);
                List<Entry> entries = new List<Entry>();
                foreach (var item in stringArray)
                {
                    entries.Add(JsonConvert.DeserializeObject<Entry>(item));
                }
                if (entries.Count > 0)
                {
                    NoRuns.Visibility = Visibility.Collapsed;
                    entries.Sort((x, y) => TimeSpan.Compare(x.TimeScore, y.TimeScore));
                    foreach (var item in entries)
                    {
                        DataEntity entity = new DataEntity(item);
                        dataModel.Data.Add(entity);
                        LeaderboardList.Items.Refresh();
                    }
                }
                else if (selectedItem != "Select a game")
                {
                    NoRuns.Visibility = Visibility.Visible;
                }
            }
            else if (selectedItem != "Select a game")
            {
                NoRuns.Visibility = Visibility.Visible;
            }
        }



        private void LeaderboardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
            {
                var item = (DataEntity)e.AddedItems[0];
                if (Mouse.RightButton == MouseButtonState.Pressed)
                {
                    if (LocalPublic.Text == "Local")
                    {
                        var result = MessageBox.Show("Click 'OK' to delete this run locally.", "Delete", MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            var entries = File.ReadAllLines(path + Games.Text).ToList();
                            for (int i = 0; i < entries.Count; i++)
                            {
                                if (item.Entry.ID == JsonConvert.DeserializeObject<Entry>(entries[i]).ID)
                                {
                                    entries.RemoveAt(i);
                                }
                            }
                            File.WriteAllLines(path + Games.Text, entries);
                            Task.Run(() => Dispatcher.BeginInvoke(new Action(() => LoadLeaderboard(LocalPublic.Text, Games.Text))));
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(Settings.Default.AdminToken))
                    {
                        var guid = LeaderboardController.GetGuidFromID(item.Entry.ID.ToString(), Settings.Default.AdminToken);
                        var result = MessageBox.Show("Click 'OK' to delete this run from the database.", "Delete", MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            LeaderboardController.DeleteEntry(guid.ToString(), item.Name);
                            Task.Run(() => Dispatcher.BeginInvoke(new Action(() => LoadLeaderboard(LocalPublic.Text, Games.Text))));
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(item.VideoLink))
                {
                    Process.Start(item.VideoLink);
                }
                LeaderboardList.SelectedItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Games.SelectedItem = null;
            Games.Text = "Select a game";
            dataModel.Data.Clear();
            LeaderboardList.Items.Refresh();
            LocalPublic.Text = "Public";
            NoRuns.Visibility = Visibility.Collapsed;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid.Focus();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
