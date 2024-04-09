using console_v3.res.entities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    public class Entity
    {
        public static Font MidFont => GraphicsManager.MidFont;

        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public string Name = null;
        public int DBRef;
        public vecf Position = vecf.Zero;
        public float X { get => Position.x; set => Position.x = value; }
        public float Y { get => Position.y; set => Position.y = value; }
        public float TileX => (int)(X / GraphicsManager.TileSize);
        public float TileY => (int)(Y / GraphicsManager.TileSize);
        public vec Tile => (TileX, TileY).V();

        public Statistics Stats = null;
        public Inventory Inventory = null;

        public Entity()
        {
        }
        public Entity(vecf position, bool addToCurrentChunkEntities = true)
        {
            Position = position;
            Initialize(addToCurrentChunkEntities);
        }
        private void Initialize(bool addToCurrentChunkEntities = true)
        {
            if(addToCurrentChunkEntities)
                Core.CurrentEntities.Add(this);
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(Graphics g)
        {
            GraphicsManager.Draw(g, DB.GetTexture(DBRef), Position);
        }

        public virtual void DrawHint(Graphics gui)
        {
            var text = Name ?? (this == Core.Instance.TheGuy ? "You" : DB.DefineName(DBRef));
            if (string.IsNullOrWhiteSpace(text))
                return;
            var font = MidFont;
            var sz = TextRenderer.MeasureText(text, font);
            var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
            var brush = Brushes.Black;
            gui.DrawString(text, font, brush, position);
        }

        public virtual void TickSecond() {}

        public virtual void Action(Entity triggerer) { }
    }
}
