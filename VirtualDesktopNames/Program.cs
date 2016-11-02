using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using WindowsDesktop;

namespace VirtualDesktopNames
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "set")
            {
                SetCurrentDesktopName();
            }
            else
            {
                ShowCurrentDesktopName();
            }
        }

        private static void SetCurrentDesktopName()
        {
            var desktops = VirtualDesktop.GetDesktops();
            var currentDesktopIndex = Array.FindIndex(desktops, x => x.Id == VirtualDesktop.Current.Id);

            var desktopNamesSettings = File.ReadAllLines(GetAssemblyDirectory() + "\\desktop-names.txt").ToList();
            while (desktopNamesSettings.Count < desktops.Length)
            {
                desktopNamesSettings.Add($"{desktopNamesSettings.Count + 1}=");
            }

            Console.Write("Enter desktop name: ");
            var name = Console.ReadLine();

            desktopNamesSettings[currentDesktopIndex] = $"{currentDesktopIndex + 1}={name}";
            File.WriteAllLines(GetAssemblyDirectory() + "\\desktop-names.txt", desktopNamesSettings.ToArray());
        }

        private static void ShowCurrentDesktopName()
        {
            var desktops = VirtualDesktop.GetDesktops();
            var currentDesktopIndex = Array.FindIndex(desktops, x => x.Id == VirtualDesktop.Current.Id);

            var desktopNamesSettings = File.ReadAllLines(GetAssemblyDirectory() + "\\desktop-names.txt").ToList();
            if (desktopNamesSettings.Count > currentDesktopIndex)
            {
                var desktopName = desktopNamesSettings[currentDesktopIndex].Split('=')[1];
                if (desktopName == string.Empty)
                {
                    Console.WriteLine($"Desktop {currentDesktopIndex}");
                }
                else
                {
                    Console.WriteLine(desktopName);
                }
            }
            else
            {
                Console.WriteLine($"Desktop {currentDesktopIndex}");
            }

            Thread.Sleep(2000);
        }

        private static string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}