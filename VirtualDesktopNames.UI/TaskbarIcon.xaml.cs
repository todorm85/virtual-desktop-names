using System;
using System.Collections.Generic;
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
        private List<CancellationTokenSource> cancellationTokens = new List<CancellationTokenSource>();

        private TaskbarIconViewModel viewModel;

        public MainWindow()
        {
            this.viewModel = new TaskbarIconViewModel();
            this.DataContext = this.viewModel;
            this.viewModel.DesktopChanged += ViewModel_DesktopChanged;

            InitializeComponent();
        }

        private void ViewModel_DesktopChanged(object sender, EventArgs e)
        {
            this.ShowStandardBalloon(this.viewModel.CurrentDesktopName);
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
    }
}