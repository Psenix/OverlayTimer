using OverlayTimer.Controllers;
using OverlayTimer.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OverlayTimer
{
    public partial class SelectPage : Page
    {
        List<string> games;

        public SelectPage()
        {
            InitializeComponent();
            Task task = new Task(GetAvailableGames);
            task.Start();
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (games.Contains(Options.Text))
            {
                GlobalXAML.MainWindow.Hide();
                Timer timer = new Timer(Options.Text);
                timer.Show();
            }
            else
            {
                DoneBtn.IsEnabled = false;
            }
        }

        private void GetAvailableGames()
        {
            games = GamesController.GetGames();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Options.Items.RemoveAt(0);
                foreach (var game in games)
                {
                    Options.Items.Add(new ComboBoxItem() { Content = game });
                }
            }));
        }

        private void Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoneBtn.IsEnabled = true;
        }

        private void Options_GotFocus(object sender, RoutedEventArgs e)
        {
            Options.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }
    }
}
