using OverlayTimer.Global;
using OverlayTimer.Pages;
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

        public MainPage()
        {
            InitializeComponent();
            GlobalXAML.MainPage = this;
            GetUsername();
            GenerateKey();
        }

        private void GenerateKey()
        {
            if (!File.Exists(path + "key"))
            {
                File.WriteAllText(path + "key", Guid.NewGuid().ToString());
            }
        }

        private void GetUsername()
        {
            var name = "Anonymous";
            if (File.Exists(path + "username"))
            {
                name = File.ReadAllText(path + "username");
            }
            NameTextBox.Text = name;
        }

        private void NewTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = File.ReadAllText(path + "username");
            if (IsValidName(name))
            {
                GlobalXAML.MainWindow.MainFrame.NavigationService.Navigate(new SelectPage());
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
                if (NameTextBox.Text.ToLower() == File.ReadAllText(path + "username").ToLower())
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
            GlobalXAML.MainWindow.MainFrame.Navigate(leaderboardPage);
        }


        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E))
            {
                GlobalXAML.MainWindow.MainFrame.Navigate(new ModeratorPage());
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid.Focus();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.Navigate(new SettingsPage());
        }

        private void CancelInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "Anonymous";
            GetUsername();
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
                    GetUsername();
                    MessageBox.Show("Name contains invalid characters");
                }));
            }
            else if(name.Length > 15 || name.Length < 3 || string.IsNullOrWhiteSpace(name))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetUsername();
                    MessageBox.Show("Name is either too short or too long.");
                }));
            }
            else
            {
                if (LeaderboardController.IsNameTaken(name))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        GetUsername();
                        MessageBox.Show("Name is already taken.");
                    }));
                }
                else
                {
                    try
                    {
                        LeaderboardController.ClaimName(name, File.ReadAllText(path + "key"));
                        File.WriteAllText(path + "username", name);
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            GetUsername();
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
