﻿using System;
using System.Threading;
using VirtualDesktopNames.Core;

namespace VirtualDesktopNames.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var vdm = new VirtualDesktopsManager();

            if (args.Length > 0 && args[0] == "set")
            {
                Console.Write("Enter desktop name: ");
                var name = Console.ReadLine();

                vdm.SetCurrentDesktopName(name);
            }
            else
            {
                Console.WriteLine(vdm.GetCurrentDesktopName());
                Thread.Sleep(2000);
            }
        }
    }
}