using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp25
{
    public class Entity
    {
        public static Font font = new Font("Segoe UI", 10F);

        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public string Name = null;
        public int Tex;
        public vecf Position = vecf.Zero;
        public vecf Offset = vecf.Zero;
        public float X { get => Position.x; set => Position.x = value; }
        public float Y { get => Position.y; set => Position.y = value; }
        public float TileX => (int)(X / 64);
        public float TileY => (int)(Y / 64);
        public vec Tile => (TileX, TileY).V();
        public List<Entity> Linked = new List<Entity>();

        public Entity()
        {
        }
        public Entity(string name, DB.Tex tex, int tile_x, int tile_y)
        {
            Name = name;
            Tex = (int)tex;
            var tsz = Core.TileSize;
            Position = (tile_x * tsz, tile_y * tsz).Vf();
            Core.Instance.Entities.Add(this);
        }

        public Entity[] GetLinked() => new[] { this }.Concat(Linked).ToArray();

        public virtual void Update()
        {
        }

        public virtual void Draw(Graphics g)
        {
            g.DrawImage(DB.GetTex(Tex), Position.pt.PlusF(Offset));
            g.FillRectangle(Brushes.Red, X - 2, Y - 2, 4, 4);
        }

        public virtual void DrawHint(Graphics gui)
        {
            var text = DB.GetName(Tex);
            if (string.IsNullOrWhiteSpace(text))
                return;
            var sz = TextRenderer.MeasureText(text, font);
            var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
            var brush = Brushes.Black;
            gui.DrawString(text, font, brush, position);
        }

        public virtual void Tick() {}

        public virtual void Action(Entity triggerer) { }
    }
}
