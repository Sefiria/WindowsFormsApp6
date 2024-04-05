using console_v2.res.entities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace console_v2
{
    public class Entity
    {
        protected int m_DBRef;
        public int DBRef
        {
            get => m_DBRef;
            set
            {
                m_DBRef = value;
                ResetGraphicsRefs();
            }
        }

        protected virtual void ResetGraphicsRefs()
        {
            (CharToDisplay, DBResSpe) = DB.RetrieveDBResOrSpe(m_DBRef);
        }

        public static Font MidFont => GraphicsManager.MidFont;
        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public string Name = null;
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
            var text = (Name ?? DB.DefineName(DBRef)) ?? (this == Core.Instance.TheGuy ? "You" : null);
            if (string.IsNullOrWhiteSpace(text))
                return;
            var font = MidFont;
            var sz = TextRenderer.MeasureText(text, font);
            var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
            var brush = Brushes.LightGray;
            gui.DrawString(text, font, brush, position);
        }

        public virtual void TickSecond() {}

        public virtual void Action(Entity triggerer) { }
    }
}
