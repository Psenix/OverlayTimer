using OverlayTimer.Entities;
using OverlayTimer.Global;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OverlayTimer.Pages
{
    /// <summary>
    /// Interaction logic for HandleRun.xaml
    /// </summary>
    public partial class EditEntryPage : Page
    {

        readonly Entry item;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";

        public EditEntryPage(Entry item_)
        {
            InitializeComponent();
            this.item = item_;
            Title.Text = item.Username + "'s Speedrun";
            Hours.Text = (item.TimeScore.Days * 24 + item.TimeScore.Hours).ToString();
            Minutes.Text = item.TimeScore.Minutes.ToString();
            Seconds.Text = item.TimeScore.Seconds.ToString();
            Milliseconds.Text = item.TimeScore.Milliseconds.ToString();
        }

        private void Hours_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Hours.Text.Length > 3)
            {
                Hours.Text = Hours.Text.Remove(Hours.Text.Length - 1);
                Hours.CaretIndex = 3;
            }
            if (!string.IsNullOrWhiteSpace(Hours.Text) && !int.TryParse(Hours.Text, out var num))
            {
                Hours.Text = Hours.Text.Remove(Hours.Text.Length - 1);
                Hours.CaretIndex = Hours.Text.Length;
            }
        }

        private void Minutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Minutes.Text.Length > 2)
            {
                Minutes.Text = Minutes.Text.Remove(Minutes.Text.Length - 1);
                Minutes.CaretIndex = 2;
            }
            if (!string.IsNullOrWhiteSpace(Minutes.Text) && !int.TryParse(Minutes.Text, out var num))
            {
                Minutes.Text = Minutes.Text.Remove(Minutes.Text.Length - 1);
                Minutes.CaretIndex = Minutes.Text.Length;
            }
        }

        private void Seconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Seconds.Text.Length > 2)
            {
                Seconds.Text = Seconds.Text.Remove(Seconds.Text.Length - 1);
                Seconds.CaretIndex = 2;
            }
            if (!string.IsNullOrWhiteSpace(Seconds.Text) && !int.TryParse(Seconds.Text, out var num))
            {
                Seconds.Text = Seconds.Text.Remove(Seconds.Text.Length - 1);
                Seconds.CaretIndex = Seconds.Text.Length;
            }
        }

        private void Milliseconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Milliseconds.Text.Length > 3)
            {
                Milliseconds.Text = Milliseconds.Text.Remove(Milliseconds.Text.Length - 1);
                Milliseconds.CaretIndex = 3;
            }
            if (!string.IsNullOrWhiteSpace(Milliseconds.Text) && !int.TryParse(Milliseconds.Text, out var num))
            {
                Milliseconds.Text = Milliseconds.Text.Remove(Milliseconds.Text.Length - 1);
                Milliseconds.CaretIndex = Milliseconds.Text.Length;
            }
        }

        private TimeSpan GetTimeSpan()
        {
            TimeSpan timeSpan = new TimeSpan();
            Dispatcher.Invoke(new Action(() => timeSpan = new TimeSpan(0, Convert.ToInt32(Hours.Text), Convert.ToInt32(Minutes.Text), Convert.ToInt32(Seconds.Text), Convert.ToInt32(Milliseconds.Text))));
            return timeSpan;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = false;
            Thread thread = new Thread(VerifyEntry);
            thread.Start();
        }

        private void VerifyEntry()
        {
            item.TimeScore = GetTimeSpan();
            LeaderboardController.VerifyEntry(item, File.ReadAllText(path + "token"));
            Dispatcher.BeginInvoke(new Action(() => GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack()));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteButton.IsEnabled = false;
            Thread thread = new Thread(DeleteEntry);
            thread.Start();

        }

        private void DeleteEntry()
        {
            LeaderboardController.DeleteEntry(item.Guid.ToString(), item.Username);
            Dispatcher.BeginInvoke(new Action(() => GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack()));
        }
    }
}
