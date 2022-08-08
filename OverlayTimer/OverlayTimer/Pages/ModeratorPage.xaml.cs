using OverlayTimer.Models;
using OverlayTimer.Properties;
using System;
using System.Diagnostics;
using System.Threading;
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
                list.Sort((x, y) => DateTime.Compare(x.SubmitDate, y.SubmitDate));
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
                    this.NavigationService.Navigate(new EditEntryPage(item.Entry));
                    GC.Collect();
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
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(Settings.Default.AdminToken))
            {
                token = Settings.Default.AdminToken;
                if (LeaderboardController.IsValidToken(token))
                {
                    Task.Run(LoadLeaderboard);
                }
                else
                {
                    LeaderboardGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                    MessageBox.Show("Token is not correct, please re-enter the token.", "Invalid token");
                }
            }
            else
            {
                LeaderboardGrid.Visibility = Visibility.Collapsed;
                LoginGrid.Visibility = Visibility.Visible;
            }
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
            ConfirmBtn.IsEnabled = false;
            Thread thread = new Thread(CheckToken);
            thread.Start();

        }

        private void CheckToken()
        {
            string token = string.Empty;
            Dispatcher.Invoke(new Action(() => token = TokenTextBox.Text));
            if (LeaderboardController.IsValidToken(token))
            {
                Settings.Default.AdminToken = token;
                Settings.Default.Save();
                this.token = token;
                LoadLeaderboard();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LeaderboardGrid.Visibility = Visibility.Visible;
                    LoginGrid.Visibility = Visibility.Collapsed;
                }));
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("Entered token is not correct, please try again.", "Invalid token")));
            }
            Dispatcher.BeginInvoke(new Action(() => ConfirmBtn.IsEnabled = true));
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