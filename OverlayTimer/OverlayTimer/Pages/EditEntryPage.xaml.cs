using OverlayTimer.Entities;
using OverlayTimer.Properties;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OverlayTimer.Pages
{
    public partial class EditEntryPage : Page
    {

        readonly Entry item;

        public EditEntryPage(Entry item_)
        {
            InitializeComponent();
            this.item = item_;
            TitleTextBlock.Text = item.Username + "'s Speedrun:";
            CategoryTextBlock.Text = item.Category;
            Hours.Text = (item.TimeScore.Days * 24 + item.TimeScore.Hours).ToString();
            Minutes.Text = item.TimeScore.Minutes.ToString();
            Seconds.Text = item.TimeScore.Seconds.ToString();
            string milliseconds = item.TimeScore.Milliseconds.ToString();
            while (milliseconds.Length < 3)
            {
                milliseconds = "0" + milliseconds;
            }
            Milliseconds.Text = milliseconds;
        }

        private void Hours_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Hours.Text.Length > 3)
            {
                Hours.Text = Hours.Text.Remove(Hours.Text.Length - 1);
                Hours.CaretIndex = 3;
            }
            if (!string.IsNullOrWhiteSpace(Hours.Text) && !int.TryParse(Hours.Text, out _))
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
            if (!string.IsNullOrWhiteSpace(Minutes.Text) && !int.TryParse(Minutes.Text, out _))
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
            if (!string.IsNullOrWhiteSpace(Seconds.Text) && !int.TryParse(Seconds.Text, out _))
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
            if (!string.IsNullOrWhiteSpace(Milliseconds.Text) && !int.TryParse(Milliseconds.Text, out _))
            {
                Milliseconds.Text = Milliseconds.Text.Remove(Milliseconds.Text.Length - 1);
                Milliseconds.CaretIndex = Milliseconds.Text.Length;
            }
        }

        private TimeSpan GetTimeSpan()
        {
            TimeSpan timeSpan = new TimeSpan();
            string milliseconds = string.Empty;
            Dispatcher.Invoke(new Action(() => milliseconds = Milliseconds.Text));
            while (milliseconds.Length < 3)
            {
                milliseconds += "0";
            }
            Dispatcher.Invoke(new Action(() => timeSpan = new TimeSpan(0, Convert.ToInt32(Hours.Text), Convert.ToInt32(Minutes.Text), Convert.ToInt32(Seconds.Text), Convert.ToInt32(milliseconds))));
            return timeSpan;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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
            LeaderboardController.VerifyEntry(item, Settings.Default.AdminToken);
            Dispatcher.BeginInvoke(new Action(() => this.NavigationService.GoBack()));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteButton.IsEnabled = false;
            Thread thread = new Thread(DeleteEntry);
            thread.Start();

        }

        private void DeleteEntry()
        {
            LeaderboardController.DeleteEntry(item.SecretID.ToString(), item.Username);
            Dispatcher.BeginInvoke(new Action(() => this.NavigationService.GoBack()));
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