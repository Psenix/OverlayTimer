using OverlayTimer.Pages;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace OverlayTimer
{
    public partial class MainPage : Page
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        readonly MainWindow mainWindow;
        Timer timer = new Timer();

        public MainPage(MainWindow mainWindow_)
        {
            InitializeComponent();
            mainWindow = mainWindow_;
            if (File.Exists(path + "username"))
            {
                NameTextBox.Text = File.ReadAllText(path + "username");
            }
        }

        private void NewTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.mainWindow = mainWindow;
            timer.mainPage = this;
            SelectPage selectPage = new SelectPage(mainWindow, File.ReadAllText(path + "username"), timer);
            mainWindow.MainFrame.NavigationService.Navigate(selectPage);
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
            LeaderboardPage leaderboardPage = new LeaderboardPage();
            mainWindow.MainFrame.Navigate(leaderboardPage);
        }
    }
}
