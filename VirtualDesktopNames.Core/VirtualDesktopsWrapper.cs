using System;
using System.Linq;
using WindowsDesktop;

namespace VirtualDesktopNames.Core
{
    public class VirtualDesktopsWrapper
    {
        public event EventHandler OnDesktopChanged;

        public VirtualDesktopsWrapper()
        {
            VirtualDesktop.CurrentChanged += VirtualDesktop_CurrentChanged;
        }

        public VirtualDesktop CurrentDesktop
        {
            get
            {
                var current = VirtualDesktop.Current;
                var currentId = current.Id;
                var desktops = this.GetDesktops();
                return desktops.First(x => x.Id == currentId);
            }
        }

        public VirtualDesktop[] GetDesktops()
        {
            return VirtualDesktop.GetDesktops();
        }

        public int GetDesktopIndex(VirtualDesktop desktop)
        {
            return Array.IndexOf(this.GetDesktops(), desktop);
        }

        private void VirtualDesktop_CurrentChanged(object sender, VirtualDesktopChangedEventArgs e)
        {
            this.OnDesktopChanged?.Invoke(this, new EventArgs());
        }
    }
}