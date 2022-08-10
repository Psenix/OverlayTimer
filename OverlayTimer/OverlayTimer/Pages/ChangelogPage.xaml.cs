using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OverlayTimer.Pages
{
    public partial class ChangelogPage : Page
    {
        public ChangelogPage()
        {
            InitializeComponent();
            HttpClient client = new HttpClient();
            string[] lines = client.GetStringAsync("https://raw.githubusercontent.com/Psenix/OverlayTimer/main/Changelog.md").Result.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                while (lines[i].StartsWith("#"))
                {
                    lines[i] = lines[i].Remove(0, 1);
                }
                lines[i] += Environment.NewLine;
            }
            ChangelogTextBlock.Text = string.Concat(lines);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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
