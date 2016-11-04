using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace VirtualDesktopNames.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VirtualDesktopsManager vdm;

        public MainWindow()
        {
            InitializeComponent();

            this.vdm = new VirtualDesktopsManager();
            vdm.OnDesktopChanged += Vdm_OnDesktopChanged;

            var name = this.vdm.GetCurrentDesktopName();
            TrayPopupTB.Text = name;
            MyNotifyIcon.ToolTipText = name;
        }
        private string currentDesktopName;
        private List<CancellationTokenSource> cancellationTokens = new List<CancellationTokenSource>();

        private void Vdm_OnDesktopChanged(object sender, System.EventArgs e)
        {
            this.currentDesktopName = this.vdm.GetCurrentDesktopName();
            this.ShowStandardBalloon(this.currentDesktopName);
            BindData();
        }

        private void BindData()
        {
            Dispatcher.Invoke((Action)(() => TrayPopupTB.Text = this.currentDesktopName));
            Dispatcher.Invoke((Action)(() => MyNotifyIcon.ToolTipText = this.currentDesktopName));
        }

        private void ShowStandardBalloon(string msg)
        {
            this.cancellationTokens.ForEach(x => x.Cancel());
            string title = msg;
            string text = "...";

            MyNotifyIcon.HideBalloonTip();
            MyNotifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);

            var taskCancellationTokenSource = new CancellationTokenSource();
            this.cancellationTokens.Add(taskCancellationTokenSource);
            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1500);
                if (!taskCancellationTokenSource.IsCancellationRequested)
                {
                    MyNotifyIcon.HideBalloonTip();
                }
            });

            task.GetAwaiter().OnCompleted(() =>
            {
                task.Dispose();
                this.cancellationTokens.Remove(taskCancellationTokenSource);
                taskCancellationTokenSource.Dispose();
            });
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void SetName_Button_Click(object sender, RoutedEventArgs e)
        {
            DesktopNameChange inputDialog = new DesktopNameChange("Please enter current desktop name:", this.vdm.GetCurrentDesktopName());
            if (inputDialog.ShowDialog() == true)
            {
                this.vdm.SetCurrentDesktopName(inputDialog.Answer);
                this.currentDesktopName = this.vdm.GetCurrentDesktopName();
                BindData();
            }
        }
    }
}