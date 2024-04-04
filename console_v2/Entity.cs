using System;
using System.Drawing;
using System.Linq;
using Tooling;

namespace console_v2
{
    public class Entity
    {
        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public vecf Position = vecf.Zero;
        public vecf Offset = vecf.Zero;
        public float X { get => Position.x; set => Position.x = value; }
        public float Y { get => Position.y; set => Position.y = value; }
        public float TileX => (int)(X / GraphicsManager.CharSize.Width);
        public float TileY => (int)(Y / GraphicsManager.CharSize.Height);
        public vec Tile => (TileX, TileY).V();
        public Bitmap Image = null, DBResSpe = null;
        public int CharToDisplay = -1;
        public Color m_CharColor = Color.White;
        public Color CharColor
        {
            get => m_CharColor;
            set { m_CharColor = value; CharBrush.Color = value; }
        }
        //CharColor
        public SolidBrush CharBrush = new SolidBrush(Color.White);

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
            if(CharToDisplay != -1)
                GraphicsManager.DrawString(g, string.Concat((char)CharToDisplay), CharBrush, Position);
            else if(DBResSpe != null)
                GraphicsManager.DrawImage(g, DBResSpe, Position);
        }

        public virtual void DrawHint(Graphics gui)
        {
            var text = Name;
            var font = MidFont;
            var sz = TextRenderer.MeasureText(text, font);
            var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
            var margin = 10;
            var rect = new Rectangle(position.Minus(margin, margin / 2), new Size(sz.Width + margin * 2, sz.Height + margin));
            byte _opacity = (byte)(opacity * byte.MaxValue).ByteCut();
            var brush = new SolidBrush(Color.FromArgb((byte)(opacity * 1.25f * byte.MaxValue).ByteCut(), Color.LightGray));
            gui.FillRectangle(new SolidBrush(Color.FromArgb(_opacity, colorLight)), rect);
            gui.DrawRectangle(new Pen(Color.FromArgb(_opacity, colorMid)), rect);
            gui.DrawString(text, font, brush, position);
        }

        public virtual void TickSecond() {}

        public virtual void Action(Entity triggerer) { }
    }
}
