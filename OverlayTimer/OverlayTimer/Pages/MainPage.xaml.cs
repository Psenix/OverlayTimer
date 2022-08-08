using OverlayTimer.Global;
using OverlayTimer.Pages;
using OverlayTimer.Properties;
using OverlayTimer.Utils;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = ModernWpf.MessageBox;

namespace OverlayTimer
{
    public partial class MainPage : Page
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        readonly LeaderboardPage leaderboardPage = new LeaderboardPage();
        readonly SettingsPage settingsPage = new SettingsPage();

        public MainPage()
        {
            InitializeComponent();
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "LocalLeaderboard");
            InitializeRTC();
            GenerateUserID();
            NameTextBox.Text = Settings.Default.Username;
            GlobalXAML.MainPage = this;
        }

        private void InitializeRTC()
        {
            if (Settings.Default.DiscordRPC)
                RPC.Initialize();
        }

        private void GenerateUserID()
        {
            if (Settings.Default.UserID == Guid.Empty)
            {
                Settings.Default.UserID = Guid.NewGuid();
                Settings.Default.Save();
            }

        }

        private void NewTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidName(Settings.Default.Username))
            {
                this.NavigationService.Navigate(new SelectPage());
                GC.Collect();
            }
            else
            {
                MessageBox.Show("Your name is not valid: must be between 3-15 characters long.");
            }
        }

        private static bool IsValidName(string name)
        {
            if (name.Length > 15 || name.Length < 3 || string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            string allowableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_.";
            foreach (char c in name)
            {
                if (!allowableLetters.Contains(c.ToString()))
                    return false;
            }
            return true;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.IsFocused)
            {
                if (NameTextBox.Text.ToLower() == Settings.Default.Username.ToLower())
                {
                    CancelInfoBtn.Visibility = Visibility.Collapsed;
                    SaveInfoBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CancelInfoBtn.Visibility = Visibility.Visible;
                    SaveInfoBtn.Visibility = Visibility.Visible;
                }
            }
        }

        private bool CharactersAllowed(string name)
        {
            string allowableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_.";
            foreach (char c in name)
            {
                if (!allowableLetters.Contains(c.ToString()))
                    return false;
            }
            return true;

        }

        private void LeaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(leaderboardPage);
            GC.Collect();
        }


        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E))
            {
                this.NavigationService.Navigate(new ModeratorPage());
                GC.Collect();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid.Focus();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(settingsPage);
            GC.Collect();
        }

        private void CancelInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "Anonymous";
            NameTextBox.Text = Settings.Default.Username;
            CancelInfoBtn.Visibility = Visibility.Collapsed;
            SaveInfoBtn.Visibility = Visibility.Collapsed;
        }

        private void SaveInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelInfoBtn.IsEnabled = false;
            SaveInfoBtn.IsEnabled = false;
            NameTextBox.IsEnabled = false;
            Thread thread = new Thread(VerifyName);
            thread.Start();
        }

        private void VerifyName()
        {
            string name = string.Empty;
            Dispatcher.Invoke(new Action(() => name = NameTextBox.Text));
            if (!CharactersAllowed(name))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    NameTextBox.Text = Settings.Default.Username;
                    MessageBox.Show("Name contains invalid characters");
                }));
            }
            else if (name.Length > 15 || name.Length < 3 || string.IsNullOrWhiteSpace(name))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    NameTextBox.Text = Settings.Default.Username;
                    MessageBox.Show("Name is either too short or too long.");
                }));
            }
            else
            {
                if (LeaderboardController.IsNameTaken(name))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        NameTextBox.Text = Settings.Default.Username;
                        MessageBox.Show("Name is already taken.");
                    }));
                }
                else
                {
                    try
                    {
                        LeaderboardController.ClaimName(name, Settings.Default.UserID.ToString());
                        Settings.Default.Username = name;
                        Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            NameTextBox.Text = Settings.Default.Username;
                            MessageBox.Show(ex.Message);
                        }));
                    }
                }
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CancelInfoBtn.IsEnabled = true;
                SaveInfoBtn.IsEnabled = true;
                NameTextBox.IsEnabled = true;
                CancelInfoBtn.Visibility = Visibility.Collapsed;
                SaveInfoBtn.Visibility = Visibility.Collapsed;
            }));
        }
    }
}
