using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brushes = System.Drawing.Brushes;
using Rectangle = System.Drawing.Rectangle;

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
            MainPage mainPage = new MainPage(this);
            MainFrame.NavigationService.Navigate(mainPage);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
