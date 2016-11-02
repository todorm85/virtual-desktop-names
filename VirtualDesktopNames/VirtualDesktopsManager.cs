using System;
using System.Linq;
using System.Threading;
using WindowsDesktop;

namespace VirtualDesktopNames
{
    public class VirtualDesktopsManager
    {
        internal VirtualDesktopsWrapper VdWrapper { get; set; }
        public VirtualDesktopsData Data { get; set; }

        public VirtualDesktopsManager()
        {
            this.Data = new VirtualDesktopsData();
            this.VdWrapper = new VirtualDesktopsWrapper();
        }

        public void SetCurrentDesktopName(string name)
        {
            var desktopsData = this.Data.GetDesktops();
            var currentWindowsDesktop = this.VdWrapper.CurrentDesktop;
            var currentDesktopDataModel = desktopsData.FirstOrDefault(x => x.Id == currentWindowsDesktop.Id);
            if (currentDesktopDataModel != null)
            {
                currentDesktopDataModel.Name = name;
                this.Data.SaveDesktop(currentDesktopDataModel);
            }
            else
            {
                this.Data.AddDesktop(new DataModels.VirtualDesktopDataModel() { Id = currentWindowsDesktop.Id, Name = name });
            }
        }

        internal string GetCurrentDesktopName()
        {
            var currentWindowsDesktop = this.VdWrapper.CurrentDesktop;
            var currentDesktopDataModel = this.Data.GetDesktops().FirstOrDefault(x => x.Id == currentWindowsDesktop.Id);
            if (currentDesktopDataModel != null && currentDesktopDataModel.Name != null)
            {
                return currentDesktopDataModel.Name;
            }
            else
            {
                // the way windows names it
                return $"Desktop {this.VdWrapper.GetDesktopIndex(currentWindowsDesktop)}";
            }
        }
    }
}