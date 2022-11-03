using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp6.World.Blocs.Tool;
using WindowsFormsApp6.World.Ores;
using WindowsFormsApp6.World.WorldResources;

namespace WindowsFormsApp6
{
    [Serializable]
    public class Inventory
    {
        public List<Item<ToolPickaxe>> Pickaxes { get; set; } = new List<Item<ToolPickaxe>>();
        public List<Item<BasicResource>> BasicResources { get; set; } = new List<Item<BasicResource>>();
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
        public void AddResource<T>(T item, int count = 1) where T : BasicResource
        {
            if (BasicResources.Any(x => x.ItemRef is T))
                BasicResources.First(x => x.ItemRef is T).Count++;
            else
                BasicResources.Add(new Item<BasicResource>(item, count));
        }
        public void UsePickaxe(ToolPickaxe pickaxe)
        {
            var p = Pickaxes.FirstOrDefault(x => x.ItemRef == pickaxe);
            if (p != null)
                UsedPickaxeID = Pickaxes.IndexOf(p);
        }
    }
}
