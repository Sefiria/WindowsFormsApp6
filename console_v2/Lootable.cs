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

        public Lootable() : base() { }
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
            if(Content.Count == 1 && Items.Count == 0 && Tools.Count == 0) CharToDisplay = DB.Resources[Content[0]];
            else if (Content.Count == 0 && Items.Count == 1 && Tools.Count == 0) CharToDisplay = DB.Resources[Items[0].DBItem];
            else if (Content.Count == 0 && Items.Count == 0 && Tools.Count == 1) CharToDisplay = DB.Resources[Tools[0].DBItem];
            else CharToDisplay = '▄';
        }

        public override void Update()
        {
            // particules ?
        }

        public override void Draw(Graphics g)
        {
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
