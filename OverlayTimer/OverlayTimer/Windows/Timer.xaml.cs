using Indieteur.GlobalHooks;
using OverlayTimer.Global;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace OverlayTimer
{
    public partial class Timer : Window
    {
        bool userClosed = true;
        TimeSpan time;
        readonly string category;
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OverlayTimer\\";
        readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        readonly TimerModel timerModel = new TimerModel();
        Key startStopKey = Key.NumPad0;
        Key resetKey = Key.NumPad9;

        public Timer(string category_)
        {
            InitializeComponent();
            category = category_;
            TimerTitle.Text = category;
            GlobalVariables.GlobalKeyHook.OnKeyDown += GlobalKeyHook_OnKeyDown;
            TimeBlock.DataContext = timerModel;
            timerModel.Timer = "00:00:00";
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            ShowInTaskbar = false;
            Left = 0;
            Top = 0;
            if (File.Exists(path + "coordinates"))
            {
                string[] coords = File.ReadAllText(path + "coordinates").Split(';');
                Left = Convert.ToDouble(coords[0]);
                Top = Convert.ToDouble(coords[1]);
            }
        }

        private void GlobalKeyHook_OnKeyDown(object sender, GlobalKeyEventArgs e)
        {
            KeyConverter keyConverter = new KeyConverter();
            if (File.Exists(path + "StartStopHotkey"))
            {
                startStopKey = (Key)keyConverter.ConvertFromString(File.ReadAllText(path + "StartStopHotkey"));
            }
            if(File.Exists(path + "ResetHotkey"))
            {
                resetKey = (Key)keyConverter.ConvertFromString(File.ReadAllText(path + "ResetHotkey"));
            }

            if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(startStopKey))
            {
                if (timer.Enabled)
                {
                    GlobalVariables.GlobalKeyHook.OnKeyDown -= GlobalKeyHook_OnKeyDown;
                    timer.Stop();
                    SubmitPage submitPage = new SubmitPage(time, category);
                    MainWindow main = GlobalXAML.MainWindow;
                    main.MainFrame.Navigate(submitPage);
                    main.Show();
                    main.Activate();
                    timerModel.Timer = "00:00:00";
                    time = new TimeSpan(0, 0, 0);
                    userClosed = false;
                    Close();
                }
                else
                {
                    timer.Start();
                }

            }
            else if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(resetKey))
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
            else if (time < TimeSpan.FromHours(24))
            {
                timerModel.Timer = string.Format("{0:hh\\:mm\\:ss}", time);
            }
            else
            {
                string timeStr = ((int)time.TotalHours).ToString() + ":" + string.Format("{0:mm\\:ss}", time);
                timerModel.Timer = timeStr;
            }
        }


        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    this.DragMove();
                    File.WriteAllText(path + "coordinates", Left + ";" + Top);
                }
                catch
                {

                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (userClosed)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}