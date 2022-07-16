using OverlayTimer.Global;
using System;
using System.IO;
using System.Windows;

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
    }
}
