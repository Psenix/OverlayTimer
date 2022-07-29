using OverlayTimer.Global;
using OverlayTimer.Pages;
using System;
using System.IO;
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
            if (File.Exists(path + "username"))
            {
                NameTextBox.Text = File.ReadAllText(path + "username");
            }
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
            if (NameTextBox.IsFocused == true)
            {
                string name = NameTextBox.Text;
                if (CharactersAllowed(name))
                {
                    File.WriteAllText(path + "username", name);
                }
                else
                {
                    try
                    {
                        NameTextBox.Text = File.ReadAllText(path + "username");
                        NameTextBox.CaretIndex = name.Length;
                    }
                    catch
                    {
                        NameTextBox.Text = "Anonymous";
                    }
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
    }
}
