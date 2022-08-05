using OverlayTimer.Global;
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

namespace OverlayTimer.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        bool programmatically = true;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        Key startStopKey;
        Key resetKey;

        public SettingsPage()
        {
            InitializeComponent();
            if(File.Exists(path + "ResetHotkey"))
            {
                ResetText.Text = File.ReadAllText(path + "ResetHotkey");
            }
            if(File.Exists(path + "StartStopHotkey"))
            {
                StartStopText.Text = File.ReadAllText(path + "StartStopHotkey");
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalXAML.MainWindow.MainFrame.NavigationService.GoBack();
        }

        private void StartStopText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.DeadCharProcessedKey == Key.None)
            {
                programmatically = true;
                startStopKey = e.Key;
                StartStopText.Text = startStopKey.ToString();
                File.WriteAllText(path + "StartStopHotkey", startStopKey.ToString());
            }
            programmatically = false;
        }

        private void StartStopText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(programmatically == false)
            {
                programmatically = true;
                StartStopText.Text = startStopKey.ToString();
                programmatically = false;
            }
        }

        private void ResetText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.DeadCharProcessedKey == Key.None)
            {
                programmatically = true;
                resetKey = e.Key;
                ResetText.Text = resetKey.ToString();
                File.WriteAllText(path + "ResetHotkey", resetKey.ToString());
            }
            programmatically = false;
        }

        private void ResetText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (programmatically == false)
            {
                programmatically = true;
                ResetText.Text = resetKey.ToString();
                programmatically = false;
            }
        }
    }
}
