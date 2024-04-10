using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v3
{
    public class Item : IItem, IName, IDBItem, IUniqueRef
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unnamed_Item";
        public int DBRef { get; set; }
        public bool IsConsommable = true;
        public bool IsMenuConsommable = true;
        public int Count;

        public Item()
        {
        }
        public Item(Item copy)
        {
            Name = copy.Name;
            DBRef = copy.DBRef;
            Count = copy.Count;
        }
        public Item(string name, int dbref, int count = 1)
        {
            Name = name;
            DBRef = dbref;
            Count = count;
        }
        public Item(int dbref, int count = 1)
        {
            Name = DB.DefineName(dbref);
            DBRef = dbref;
            Count = count;
        }

        public void Use()
        {
            if (DBRef.IsItem())
                return;
            else if(DBRef.IsConsumable())
                Consume();
            else if(DBRef.IsStructure())
                Place();
        }

        public void Consume()
        {
            if (Count == 0) return;

            //

            Count--;
        }

        public void Place()
        {
            Core.Instance.SceneAdventure.ItemToPlace = this;
            Core.Instance.SwitchScene(Core.Scenes.Adventure);
        }
    }
}
