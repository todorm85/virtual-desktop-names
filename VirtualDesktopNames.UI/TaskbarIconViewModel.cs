using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualDesktopNames.Client.MVVMBaseClasses;
using VirtualDesktopNames.Core;

namespace VirtualDesktopNames.Client
{
    public class TaskbarIconViewModel : ObservableObject
    {
        private string currentDesktopName;
        private VirtualDesktopsManager vdm;

        public TaskbarIconViewModel()
        {
            this.vdm = new VirtualDesktopsManager();
            this.CurrentDesktopName = this.vdm.GetCurrentDesktopName();
            vdm.OnDesktopChanged += Vdm_OnDesktopChanged;
        }

        public string CurrentDesktopName
        {
            get { return currentDesktopName; }
            set
            {
                this.RaisePropertyChangedEvent("CurrentDesktopName");
                currentDesktopName = value;
            }
        }

        public ICommand SetNameCommand
        {
            get { return new DelegateCommand(this.SetDesktopName); }
        }

        private void SetDesktopName()
        {
            DesktopNameChange inputDialog = new DesktopNameChange("Please enter current desktop name:", this.vdm.GetCurrentDesktopName());
            if (inputDialog.ShowDialog() == true)
            {
                this.vdm.SetCurrentDesktopName(inputDialog.Answer);
                this.CurrentDesktopName = this.vdm.GetCurrentDesktopName();
            }
        }

        public ICommand ExitCommand
        {
            get { return new DelegateCommand(this.Exit); }
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        private void Vdm_OnDesktopChanged(object sender, System.EventArgs e)
        {
            this.CurrentDesktopName = this.vdm.GetCurrentDesktopName();
            this.RaiseDesktopChanged();
        }

        public event EventHandler DesktopChanged;

        protected void RaiseDesktopChanged()
        {
            this.DesktopChanged?.Invoke(this, new EventArgs());
        }
    }
}
