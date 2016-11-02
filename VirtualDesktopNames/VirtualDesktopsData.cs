using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VirtualDesktopNames.DataModels;

namespace VirtualDesktopNames
{
    public class VirtualDesktopsData
    {
        private readonly string dataFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\desktop-names.txt";
        public const string SpacingSymbol = " | ";
        public VirtualDesktopsData()
        {
            if (!File.Exists(this.dataFilePath))
            {
                File.CreateText(this.dataFilePath).Close();
            }
        }

        internal IEnumerable<VirtualDesktopDataModel> GetDesktops()
        {
            var rawDesktopsData = File.ReadAllLines(this.dataFilePath);
            return rawDesktopsData.Select((x) =>
            {
                var desktopData = x.Split(new string[] { VirtualDesktopsData.SpacingSymbol }, StringSplitOptions.None);
                return new VirtualDesktopDataModel()
                {
                    Id = Guid.Parse(desktopData[0]),
                    Name = desktopData[1]
                };
            });
        }

        internal void SaveDesktop(VirtualDesktopDataModel model)
        {
            var desktops = this.GetDesktops().ToList();

            if (desktops.All(x => x.Id != model.Id))
            {
                throw new InvalidOperationException("Desktop with specified id does not exist.");
            }

            var desktop = desktops.First(x => x.Id == model.Id);
            desktop.Name = model.Name;

            this.SaveDesktops(desktops);
        }

        internal void AddDesktop(VirtualDesktopDataModel model)
        {
            var desktops = this.GetDesktops().ToList();

            if (desktops.Any(x => x.Id == model.Id))
            {
                throw new InvalidOperationException("Desktop with same id already exists.");
            }

            desktops.Add(model);

            this.SaveDesktops(desktops);
        }

        internal void SaveDesktops(IEnumerable<VirtualDesktopDataModel> desktops)
        {
            var serializedData = desktops.Select(x => $"{x.Id}{VirtualDesktopsData.SpacingSymbol}{x.Name}");
            File.WriteAllLines(this.dataFilePath, serializedData.ToArray());
        }
    }
}