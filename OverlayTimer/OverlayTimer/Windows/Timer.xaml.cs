using Indieteur.GlobalHooks;
using OverlayTimer.Global;
using OverlayTimer.Utils;
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
        DateTime startTime;
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
                    var timePassed = DateTime.Now.Subtract(startTime);
                    timer.Stop();
                    GlobalVariables.GlobalKeyHook.OnKeyDown -= GlobalKeyHook_OnKeyDown;
                    RPC.UpdateDetails("In Menu");
                    RPC.UpdateState(string.Empty);
                    RPC.HideTime();
                    SubmitPage submitPage = new SubmitPage(timePassed, category);
                    MainWindow main = GlobalXAML.MainWindow;
                    main.MainFrame.Navigate(submitPage);
                    main.Show();
                    main.Activate();
                    timerModel.Timer = "00:00:00";
                    startTime = new DateTime();
                    userClosed = false;
                    Close();
                }
                else
                {
                    startTime = DateTime.Now;
                    timer.Start();
                    RPC.UpdateDetails("Speedrunning: ");
                    RPC.UpdateState(category);
                    RPC.ShowTime();
                }

            }
            else if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(resetKey))
            {
                timer.Stop();
                timerModel.Timer = "00:00:00";
                startTime = new DateTime();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var timeSpan = DateTime.Now.Subtract(startTime);
            if (timeSpan < TimeSpan.FromHours(1))
            {
                timerModel.Timer = string.Format("{0:mm\\:ss\\:ff}", timeSpan);
            }
            else if (timeSpan < TimeSpan.FromHours(24))
            {
                timerModel.Timer = string.Format("{0:hh\\:mm\\:ss}", timeSpan);
            }
            else
            {
                string timeStr = ((int)timeSpan.TotalHours).ToString() + ":" + string.Format("{0:mm\\:ss}", timeSpan);
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