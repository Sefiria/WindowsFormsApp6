using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities;

namespace WindowsFormsApp3
{
    public static class Logger
    {
        public static bool NewLogs = false;

        public enum LogInventoryAddFrom
        {
            Unknown,
            Chest,
        }
        public static void LogInventoryAdd(Dictionary<Item, int> items, LogInventoryAddFrom from)
        {
            foreach (var item in items)
            {
                var log = $"Got {item.Key.ItemType} x{item.Value} from a {Enum.GetName(typeof(LogInventoryAddFrom), from)}";
                SharedData.Logs.Add(log);
                Console.WriteLine(log);
                NewLogs = true;
            }
        }
    }
}
