using OverlayTimer.Properties;
using OverlayTimer.Utils;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OverlayTimer.Pages
{
    public partial class SettingsPage : Page
    {
        readonly ChangelogPage changelogPage = new ChangelogPage();
        Key startStopKey;
        Key resetKey;

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void StartStopText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.DeadCharProcessedKey == Key.None)
            {
                StartStopText.TextChanged -= StartStopText_TextChanged;
                startStopKey = e.Key;
                StartStopText.Text = startStopKey.ToString();
                Settings.Default.StartStopHotkey = startStopKey.ToString();
                Settings.Default.Save();
                StartStopText.TextChanged += StartStopText_TextChanged;
            }
        }

        private void StartStopText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (StartStopText.IsFocused)
                StartStopText.Text = startStopKey.ToString();
        }

        private void ResetText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.DeadCharProcessedKey == Key.None)
            {
                resetKey = e.Key;
                ResetText.Text = resetKey.ToString();
                Settings.Default.ResetHotkey = resetKey.ToString();
                Settings.Default.Save();
            }
        }

        private void ResetText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ResetText.IsFocused)
                ResetText.Text = resetKey.ToString();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (DiscordRPC.IsFocused)
            {
                RPC.Initialize();
                Settings.Default.DiscordRPC = true;
                Settings.Default.Save();
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DiscordRPC.IsFocused)
            {
                RPC.Deinitialize();
                Settings.Default.DiscordRPC = false;
                Settings.Default.Save();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StartStopText.Text = Settings.Default.StartStopHotkey;
            ResetText.Text = Settings.Default.ResetHotkey;
            DiscordRPC.IsChecked = Settings.Default.DiscordRPC;
            VersionTextBlock.Text = "v " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(changelogPage);
        }
    }
}
