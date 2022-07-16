using OverlayTimer.Controllers;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OverlayTimer.Pages
{
    /// <summary>
    /// Interaction logic for LeaderboardPage.xaml
    /// </summary>
    public partial class LeaderboardPage : Page
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            Task task = new Task(GetAvailableGames);
            task.Start();
        }

        private void GetAvailableGames()
        {
            List<string> games = GamesController.GetGames();
            foreach (var game in games)
            {
                Dispatcher.BeginInvoke(new Action(() => Games.Items.Add(new ComboBoxItem() { Content = game })));
            }
        }
    }
}
