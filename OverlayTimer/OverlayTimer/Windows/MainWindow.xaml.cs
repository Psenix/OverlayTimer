using OverlayTimer.Global;
using OverlayTimer.Pages;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace OverlayTimer
{
    public partial class MainWindow : Window
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";

        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(path);
            if (!File.Exists(path + "username"))
            {
                File.WriteAllText(path + "username", "Anonymous");
            }
            GlobalXAML.MainWindow = this;
            MainFrame.NavigationService.Navigate(new MainPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.E)
            {
                GlobalXAML.MainWindow.MainFrame.Navigate(new ModeratorPage());
            }
        }
    }
}
