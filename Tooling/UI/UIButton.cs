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
        public Color BackgroundColor = Color.Gray, BoundsColor = Color.White;
        public bool SleepTransparent = false;
        public Action<UI> OnClick;

        public override void Click()
        {
            base.Click();
            OnClick?.Invoke(this);
        }

        public override void Draw(Graphics g)
        {
            var pos = GetGlobalPosition();
            var bc = SleepTransparent && !IsHover ? Color.FromArgb(128, BackgroundColor) : BackgroundColor;
            Brush brush = new SolidBrush(Bounds.Contains(MouseStatesV1.Position) ? bc.Mod(20 * (MouseStatesV1.IsDown ? -1 : 1)) : bc);
            g.FillRectangle(brush, Bounds);
            if(Tex != null) g.DrawImage(SleepTransparent && !IsHover ? Tex.WithOpacity(128) : Tex, pos.x + TexPos.X, pos.y + TexPos.Y);
            g.DrawRectangle(new Pen(BoundsColor, IsHover ? 2F : 1F), Bounds.ToIntRect());
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
