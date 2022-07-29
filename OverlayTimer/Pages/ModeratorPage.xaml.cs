using OverlayTimer.Global;
using OverlayTimer.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static OverlayTimer.Models.DataModel;
using MessageBox = ModernWpf.MessageBox;

namespace OverlayTimer.Pages
{
    public partial class ModeratorPage : Page
    {
        string token;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        readonly DataModel dataModel = new DataModel();

        public ModeratorPage()
        {
            InitializeComponent();
            LeaderboardList.ItemsSource = dataModel.Data;

            if (!File.Exists(path + "token"))
            {
                LeaderboardGrid.Visibility = Visibility.Collapsed;
                LoginGrid.Visibility = Visibility.Visible;
            }
            else
            {
                token = File.ReadAllText(path + "token");
                Task.Run(LoadLeaderboard);
            }
        }

        private void LoadLeaderboard()
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    dataModel.Data.Clear();
                    GetPublicLeaderboard();
                    LeaderboardList.Items.Refresh();
                }));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void GetPublicLeaderboard()
        {
            var list = LeaderboardController.GetUnverifiedEntries(token);
            if (list.Count > 0)
            {
                list.Sort((x, y) => TimeSpan.Compare(x.TimeScore, y.TimeScore));
                foreach (var item in list)
                {
                    DataEntity entity = new DataEntity(item);
                    dataModel.Data.Add(entity);
                    LeaderboardList.Items.Refresh();
                }
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
                    var result = MessageBox.Show("Click 'yes' to approve this run or click 'no' to delete the run from the database", "Edit", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        LeaderboardController.VerifyEntry(item.Entry, token);
                        Task.Run(LoadLeaderboard);
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        LeaderboardController.DeleteEntry(item.Entry.Guid.ToString(), item.Name);
                        Task.Run(LoadLeaderboard);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(item.VideoLink))
                {
                    Process.Start(item.VideoLink);
                }
                LeaderboardList.SelectedItem = null;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            dataModel.Data.Clear();
            LeaderboardList.Items.Refresh();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string token = TokenTextBox.Text;
            if (LeaderboardController.IsValidToken(token))
            {
                File.WriteAllText(path + "token", token);
                this.token = token;
                Task.Run(LoadLeaderboard);
                LeaderboardGrid.Visibility = Visibility.Visible;
                LoginGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Entered token is not correct, please try again.", "Invalid token");
            }
        }

    }
}