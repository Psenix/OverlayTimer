using OverlayTimer.Controllers;
using OverlayTimer.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OverlayTimer
{
    public partial class SelectPage : Page
    {
        Dictionary<string, string> games;

        public SelectPage()
        {
            InitializeComponent();
            Task task = new Task(GetAvailableGames);
            task.Start();
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (games.ContainsKey(Options.Text))
            {
                GlobalXAML.MainWindow.Hide();
                TimerWindow timer = new TimerWindow(Options.Text);
                timer.Show();
            }
            else
            {
                DoneBtn.IsEnabled = false;
            }
        }

        private void GetAvailableGames()
        {
            try
            {
                games = GamesController.GetGames();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Options.Items.RemoveAt(0);
                    foreach (var game in games)
                    {
                        Options.Items.Add(new ComboBoxItem() { Content = game.Key });
                    }
                }));
            }
            catch
            {
                Options.Items.Add(new ComboBoxItem() { Content = "Could not fetch data", IsEnabled = false });
            }
        }

        private void Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                DoneBtn.IsEnabled = true;
                var selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
                Rules.Text = games[selectedItem];
                RulesTitle.Text = selectedItem + " Rules";
            }
        }

        private void Options_GotFocus(object sender, RoutedEventArgs e)
        {
            Options.Text = string.Empty;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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
