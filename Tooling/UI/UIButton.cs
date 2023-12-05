using System;
using System.Drawing;

namespace Tooling.UI
{
    public class UIButton : UI
    {
        private Bitmap m_Tex;
        public Bitmap Tex
        {
            get => m_Tex;
            set
            {
                m_Tex = value;
                TexPos = m_Tex == null ? PointF.Empty : new PointF(Size.x / 2f - Tex.Width / 2F, Size.y / 2f - Tex.Height / 2F);
            }
        }
        public PointF TexPos { get; private set; }
        public Color BackgroundColor = Color.Gray;
        public Action OnClick;

        public override void Click()
        {
            base.Click();
            OnClick?.Invoke();
        }

        public override void Draw(Graphics g)
        {
            var pos = GetGlobalPosition();
            Brush brush = new SolidBrush(Bounds.Contains(MouseStates.Position) ? BackgroundColor.Mod(20 * (MouseStates.IsDown ? -1 : 1)) : BackgroundColor);
            g.FillRectangle(brush, Bounds);
            if(Tex != null) g.DrawImage(Tex, pos.x + TexPos.X, pos.y + TexPos.Y);
            g.DrawRectangle(Pens.White, Bounds.ToIntRect());
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
