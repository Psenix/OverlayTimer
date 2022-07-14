using OverlayTimer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class SelectPage : Page
    {
        readonly MainWindow mainWindow;
        Timer timer;

        public SelectPage(MainWindow mainWindow_, string userName_, Timer timer_)
        {
            InitializeComponent();
            Task task = new Task(GetAvailableGames);
            task.Start();
            mainWindow = mainWindow_;
            timer = timer_;
            timer.userName = userName_;
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.category = Options.Text;
            timer.Show();
            mainWindow.Hide();
        }

        private void GetAvailableGames()
        {
            List<string> games = GamesController.GetGames();
            foreach (var game in games)
            {
                Dispatcher.BeginInvoke(new Action(() => Options.Items.Add(new ComboBoxItem() { Content = game })));
            }
        }

        private void Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoneBtn.IsEnabled = true;
        }
    }
}
