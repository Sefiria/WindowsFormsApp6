using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;

namespace console_v2
{
    internal class Lootable : Entity
    {
        List<(int dbref, int count)> Content = new List<(int dbref, int count)>();
        List<Item> Items = new List<Item>();
        List<Tool> Tools = new List<Tool>();
        new int DBResSpe = -1;

        public Lootable() : base() { }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params (Objets obj, int count)[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content.Select(c => ((int)c.obj, c.count)));
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params (Outils obj, int count)[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Content.AddRange(content.Select(c => ((int)c.obj, c.count)));
            Initialize();
        }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params (int obj, int count)[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
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
            if (Content.Count == 1 && Items.Count == 0 && Tools.Count == 0) set(Content[0].dbref);
            else if (Content.Count == 0 && Items.Count == 1 && Tools.Count == 0) set(Items[0].DBRef);
            else if (Content.Count == 0 && Items.Count == 0 && Tools.Count == 1) set(Tools[0].DBRef);
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
