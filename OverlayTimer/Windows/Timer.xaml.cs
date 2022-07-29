using Indieteur.GlobalHooks;
using OverlayTimer.Global;
using System;
using System.Windows;
using System.Windows.Input;

namespace OverlayTimer
{
    public partial class Timer : Window
    {
        bool userClosed = true;
        TimeSpan time;
        readonly string category;
        readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        readonly TimerModel timerModel = new TimerModel();

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
        }

        private void GlobalKeyHook_OnKeyDown(object sender, GlobalKeyEventArgs e)
        {
            if (e.KeyCode == (VirtualKeycodes)KeyInterop.VirtualKeyFromKey(Key.NumPad0))
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