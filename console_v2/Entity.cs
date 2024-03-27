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
        public Bitmap Image = null;
        public int CharToDisplay = 'X';
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
            GraphicsManager.DrawString(g, string.Concat((char)CharToDisplay), CharBrush, Position);
        }

        public virtual void TickSecond() {}

        public virtual void Action(Entity triggerer) { }
    }
}
