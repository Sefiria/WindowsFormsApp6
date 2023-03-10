using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants
{
    internal class SellCatalogue
    {
        public enum ItemsName
        {
            None = 0,
        }

        public static ItemsName GetItemNameByString(string stringname)
        {
            if (ItemsString.ContainsValue(stringname))
                return ItemsString.First(x => x.Value == stringname).Key;
            return ItemsName.None;
        }

        public static Dictionary<ItemsName, string> ItemsString = new Dictionary<ItemsName, string>()
        {
            [ItemsName.None] = "None",
        };
        public static Dictionary<ItemsName, int> ItemsPrice = new Dictionary<ItemsName, int>()
        {
            [ItemsName.None] = 0,
        };
        public static Dictionary<string, int> FruitsPrice = new Dictionary<string, int>()
        {
            ["Pomme"] = 5,
            ["Tomate"] = 15,
            ["Concombre"] = 50,
        };
        public static string NameOf(ItemsName item) => ItemsString[item];
        public static int PriceOf(ItemsName item) => ItemsPrice[item];
        public static int PriceOf(string item) => FruitsPrice.ContainsKey(item) ? FruitsPrice[item] : ItemsPrice[GetItemNameByString(item)];
    }
}
