using Indieteur.GlobalHooks;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace OverlayTimer
{
    public partial class Timer : Window
    {
        GlobalKeyHook globalKeyHook = new GlobalKeyHook();
        readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        readonly TimerModel timerModel = new TimerModel();
        TimeSpan time;
        public MainWindow mainWindow;
        public MainPage mainPage;
        public string userName;
        public string category;

        public Timer()
        {
            InitializeComponent();
            globalKeyHook.OnKeyDown += GlobalKeyHook_OnKeyDown;
            TimeBlock.DataContext = timerModel;
            timerModel.Timer = "00:00:00";
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            ShowInTaskbar = false;
            Left = 0;
            Top = 0;
        }

        private void GlobalKeyHook_OnKeyDown(object sender, GlobalKeyEventArgs e)
        {
            if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(Key.NumPad0))
            {
                if (timer.Enabled)
                {
                    timer.Stop();
                    SubmitPage submitPage = new SubmitPage(time, userName, category, mainWindow, mainPage);
                    mainWindow.MainFrame.Navigate(submitPage);
                    mainWindow.Show();
                    mainWindow.Activate();
                    globalKeyHook = null;
                    timerModel.Timer = "00:00:00";
                    time = new TimeSpan(0, 0, 0);
                    this.Hide();
                }
                else
                {
                    timer.Start();
                }

            }
            else if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(Key.NumPad9))
            {
                timer.Stop();
                timerModel.Timer = "00:00:00";
                time = new TimeSpan(0, 0, 0);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 0, 0, 30));
            if (time < TimeSpan.FromHours(1))
            {
                timerModel.Timer = string.Format("{0:mm\\:ss\\:ff}", time);
            }
            else
            {
                timerModel.Timer = string.Format("{0:hh\\:mm\\:ss}", time);
            }
        }


        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    this.DragMove();
                }
                catch
                {

                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TimerTitle.Text = category;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }
    }
}