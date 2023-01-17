using System.Drawing;
using System.Windows.Forms;

namespace RPDialog
{
    public class RPDialog
    {
        private Rectangle m_RenderBounds;
        public Rectangle RenderBounds
        {
            get => m_RenderBounds;
            set
            {
                m_RenderBounds = value;
                DialogBounds = new Rectangle(0, m_RenderBounds.Height - m_RenderBounds.Height / 5, m_RenderBounds.Width, m_RenderBounds.Height / 5);
                ImageBounds = new Rectangle(0, 0, DialogBounds.Width / 5, DialogBounds.Height);
            }
        }

        public Rectangle DialogBounds { get; private set; }
        public Rectangle ImageBounds { get; private set; }

        public RPDialog() { }

        public void Update()
        {
        }

        public void Draw(Graphics g)
        {
        }
    }
}