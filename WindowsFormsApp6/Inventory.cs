using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp6.World.Blocs.Tool;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6
{
    [Serializable]
    public class Inventory
    {
        private List<Item<ToolPickaxe>> m_Pickaxes = new List<Item<ToolPickaxe>>();
        [JsonIgnore] public List<Item<ToolPickaxe>> Pickaxes { get => m_Pickaxes; set => m_Pickaxes = value; }
        public int UsedPickaxeID { get; set; } = -1;
        public ToolPickaxe GetUsedPickaxe() => Pickaxes[UsedPickaxeID].ItemRef;

        public Inventory()
        {
            var used = new ToolPickaxe(OreType.None);
            AddItem(used);
            UsePickaxe(used);
        }

        public void AddItem<T>(T item, int count = 1) where T:class
        {
            switch (item)
            {
                case ToolPickaxe pickaxe:
                    Pickaxes.Add(new Item<ToolPickaxe>(pickaxe));
                    break;
            }
        }
        public void UsePickaxe(ToolPickaxe pickaxe)
        {
            var p = Pickaxes.FirstOrDefault(x => x.ItemRef == pickaxe);
            if (p != null)
                UsedPickaxeID = Pickaxes.IndexOf(p);
        }
    }
}
