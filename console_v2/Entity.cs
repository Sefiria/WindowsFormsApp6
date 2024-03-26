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

        public Entity()
        {
            Initialize();
        }
        public Entity(vecf position)
        {
            Position = position;
            Initialize();
        }
        private void Initialize()
        {
            Core.Instance.CurrentScene.Entities.Add(this);
        }

        public virtual void Update()
        {
        }
        //public virtual void Draw(Graphics g)
        //{
        //    var u = Core.CurrentEntities.FirstOrDefault(e => e.ID == ID);
        //    g.DrawImage(Image, Position);
        //}
        public virtual void Draw(Graphics g)
        {
            g.DrawString(string.Concat((char)CharToDisplay), GraphicsManager.Font, CharBrush, Position.pt);
        }
    }
}
