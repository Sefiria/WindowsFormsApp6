using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core
{
    public class Item: IComparable
    {
        public string Name;
        public int Count;

        public Item(string Name, int Count = 1)
        {
            this.Name = Name;
            this.Count = Count;
        }

        public int CompareTo(object obj)
        {
            if (obj is string)
                return this.Name.CompareTo(obj as string);

            if (obj is Item)
                return this.Name.CompareTo((obj as Item).Name);

            return -1;
        }

        public override bool Equals(object obj) => this.CompareTo(obj) == 0;
        public static bool operator ==(Item A, object B)
        {
            if (B is Item)
                return A.Name == (B as Item).Name && A.Count == (B as Item).Count;

            return false;
        }
        public static bool operator !=(Item A, object B) => A.CompareTo(B) != 0;
        public override string ToString()
        {
            return $"{Name} {Count}";
        }
    }

    public struct NNItem
    {
        public Item item;

        public NNItem(Item item)
        {
            this.item = item;
        }

        public override string ToString()
        {
            return item.ToString();
        }
    }

    public class Data
    {
        public Data(){}

        public Map CurrentMap = null;
        public Vec CurrentRoom = Vec.Empty;
        public Vec CurrentTile = Vec.Empty;
        public Vec TargetTile = Vec.Empty;
        public List<Item> Inventory = new List<Item>();
        public List<Attacker> PlayerTeam = new List<Attacker>(4) {  new Attacker("Player", 10, 0, 0, 0, 1, 0),
                                                                    new Attacker("Test", 10, 0, 0, 0, 0, 0),
                                                                    new Attacker("Toto", 9999, 999, 999, 99, 99, 99)};

        public void SetTargetBlock(HelpBlock.Blocks block)
        {
            CurrentMap[CurrentRoom][TargetTile] = HelpBlock.GetIdFromEnum(block);
        }
        public void AddItemToInventory(Item item)
        {
            if (Inventory.Contains(item))
            {
                Inventory[Inventory.IndexOf(item)].Count += item.Count;
            }
            else
            {
                Inventory.Add(item);
            }
        }
        public void RemoveItemToInventory(NNItem item, int count = 1)
        {
            RemoveItemToInventory(item.item, count);
        }
        public void RemoveItemToInventory(Item item, int count = 1)
        {
            if (Inventory.Contains(item))
            {
                Inventory[Inventory.IndexOf(item)].Count -= count;

                if (Inventory[Inventory.IndexOf(item)].Count <= 0)
                {
                    Inventory.Remove(item);
                }
            }
        }
        public int GetItemCount(string ItemName)
        {
            Item item = GetInventoryItem(ItemName);
            return object.ReferenceEquals(item, null) ? 0 : item.Count;
        }
        public Item GetInventoryItem(string ItemName)
        {
            List<Item> found = Inventory.Where(x => x.Name == ItemName).ToList();

            if (found.Count == 0)
                return null;

            return found[0];
        }
        public int GetCurrentTile() => CurrentMap[CurrentRoom][CurrentTile];
        public int GetTargetTile() => CurrentMap[CurrentRoom][TargetTile];
    }
}
