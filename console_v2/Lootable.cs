using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    internal class Lootable : Entity
    {
        List<int> Content = new List<int>();
        List<Item> Items = new List<Item>();
        List<Tool> Tools = new List<Tool>();
        int DBResSpe = -1;

        public Lootable() : base() { }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params Objets[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content.Select(c => (int)c));
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params Outils[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content.Select(c => (int)c));
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params int[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content);
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params Item[] items) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Items.AddRange(items);
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params Tool[] tools) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Tools.AddRange(tools);
            Initialize();
        }
        public Lootable(vec tile, List<int> content, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content);
            Initialize();
        }
        void Initialize()
        {
            CharToDisplay = 0;
            void set(int dbref)
            {
                if (DB.Resources.ContainsKey(dbref))
                    CharToDisplay = DB.Resources[dbref];
                else if (DB.ResourcesSpecials.ContainsKey(dbref))
                    DBResSpe = dbref;
            }
            if (Content.Count == 1 && Items.Count == 0 && Tools.Count == 0) set(Content[0]);
            else if (Content.Count == 0 && Items.Count == 1 && Tools.Count == 0) set(Items[0].DBItem);
            else if (Content.Count == 0 && Items.Count == 0 && Tools.Count == 1) set(Tools[0].DBItem);
            if (CharToDisplay == 0 && DBResSpe == -1)
                CharToDisplay = '▄';
        }

        public override void Update()
        {
            // particules ?
        }

        public override void Draw(Graphics g)
        {
            if(DBResSpe > -1)
                g.DrawImage(DB.ResourcesSpecials[DBResSpe], SceneAdventure.DrawingRect.Location.Plus(Position.pt));
            else
                base.Draw(g);
        }

        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            if (triggerer?.Inventory != null)
            {
                Content.ForEach(c => triggerer.Inventory.Add(c));
                triggerer.Inventory.Add(Items.ToArray());
                triggerer.Inventory.Add(Tools.ToArray());
            }
            Exists = false;
        }
    }
}
