using Microsoft.Win32.TaskScheduler;
using OverlayTimer.Global;
using OverlayTimer.Properties;
using OverlayTimer.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using Action = System.Action;
using Task = System.Threading.Tasks.Task;

namespace OverlayTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(CheckForUpdate);
            UpgradeVersion(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            GlobalXAML.MainWindow = this;
            MainFrame.NavigationService.Navigate(new MainPage());
            GC.Collect();
            Activate();
        }

        private static void UpgradeVersion(string path)
        {
            GenerateUserID(path);
            Settings.Default.Upgrade();

            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string[] fileEntries = Directory.GetDirectories(Path.Combine(path, @"..\..\"));
            foreach (string filename in fileEntries)
            {
                Version savedVersion = new Version(Path.GetFileName(filename));
                if (savedVersion < currentVersion)
                    Directory.Delete(filename, true);
            }
        }
        private static void GenerateUserID(string path)
        {
            if (!File.Exists(path))
            {
                if (Settings.Default.UserID == Guid.Empty)
                {
                    Settings.Default.UserID = Guid.NewGuid();
                    Settings.Default.Save();
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            RPC.Deinitialize();
            Environment.Exit(0);
        }

        private async void CheckForUpdate()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://raw.githubusercontent.com/Psenix/OverlayTimer/main/Version");
                var result = await response.Content.ReadAsStringAsync();
                Version latestVersion = new Version(result);
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (latestVersion > currentVersion)
                {
                    MessageBoxResult messageBoxResult = MessageBoxResult.Cancel;
                    Dispatcher.Invoke(new Action(() =>
                    {
                        GlobalXAML.MainWindow.WindowState = WindowState.Minimized;
                        messageBoxResult = ModernWpf.MessageBox.ShowAsync($"Version {latestVersion} is now available.\n\nDo you want to download it?", "Update", MessageBoxButton.OKCancel).Result.GetValueOrDefault();
                    }));
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        DownloadAndInstallUpdate(client);
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(() => GlobalXAML.MainWindow.WindowState = WindowState.Normal));
                    }
                }
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    GlobalXAML.MainWindow.WindowState = WindowState.Normal;
                    ModernWpf.MessageBox.Show(e.Message, "Failed to check for update");
                }));
            }

        }

        private async void DownloadAndInstallUpdate(HttpClient client)
        {
            try
            {
                string path = Path.Combine(Path.GetTempPath(), "OverlayTimer-Installer.exe");
                byte[] result = await client.GetByteArrayAsync("https://github.com/psenix/OverlayTimer/releases/latest/download/OverlayTimer-Installer.exe");
                File.WriteAllBytes(path, result);
                RPC.Deinitialize();
                CreateScheduledTask(path);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    GlobalXAML.MainWindow.WindowState = WindowState.Normal;
                    ModernWpf.MessageBox.Show(e.Message, "Failed to install update");
                }));
            }
        }

        private static void CreateScheduledTask(string path)
        {
            TaskService ts = new TaskService();
            TaskDefinition td = ts.NewTask();
            td.Triggers.Add(new TimeTrigger(DateTime.Now.AddSeconds(5)));
            td.Actions.Add(new ExecAction(path, null, null));
            ts.RootFolder.RegisterTaskDefinition("Overlay Timer Update", td);
        }
    }
}
