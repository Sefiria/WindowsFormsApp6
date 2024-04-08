using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;

namespace console_v3
{
    internal class Lootable : Entity
    {
        List<(int dbref, int ore, int count)> Content = new List<(int dbref, int ore, int count)>();
        List<Item> Items = new List<Item>();
        List<Tool> Tools = new List<Tool>();

        public Lootable() : base() { }
        public Lootable(vec tile, bool addToCurrentChunkEntities = true, params (int obj, int ore, int count)[] content) : base(tile.ToWorld(), addToCurrentChunkEntities)
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
            if (Content.Count == 1 && Items.Count == 0 && Tools.Count == 0) DBRef = Content[0].dbref;
            else if (Content.Count == 0 && Items.Count == 1 && Tools.Count == 0) DBRef = Items[0].DBRef;
            else if (Content.Count == 0 && Items.Count == 0 && Tools.Count == 1) DBRef = Tools[0].DBRef;
            else DBRef = (int)DB.Tex.Chest;
        }

        public override void Update()
        {
            // particules ?
        }

        public override void Draw(Graphics g)
        {
            if (Content.Count == 0 && Items.Count == 0 && Tools.Count == 1)
                GraphicsManager.Draw(g, Tools[0].Image, Position);
            else
                base.Draw(g);
        }

        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            if (triggerer?.Inventory != null)
            {
                Content.ForEach(c => { if (c.ore != -1) triggerer.Inventory.AddTool((c.dbref, c.ore, c.count)); else triggerer.Inventory.AddItem((c.dbref, c.count)); });
                triggerer.Inventory.Add(Items.ToArray());
                triggerer.Inventory.Add(Tools.ToArray());
            }
            Exists = false;
        }
    }
}
