using System;
using System.Linq;
using WindowsDesktop;

namespace VirtualDesktopNames
{
    public class VirtualDesktopsWrapper
    {
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
    }
}