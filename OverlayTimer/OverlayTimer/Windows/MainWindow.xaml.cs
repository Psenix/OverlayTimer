using OverlayTimer.Global;
using OverlayTimer.Utils;
using System;
using System.Windows;

namespace OverlayTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GlobalXAML.MainWindow = this;
            MainFrame.NavigationService.Navigate(new MainPage());
            GC.Collect();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            RPC.Deinitialize();
            Environment.Exit(0);
        }
    }
}
