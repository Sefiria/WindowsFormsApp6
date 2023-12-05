using System;
using System.Drawing;

namespace Tooling.UI
{
    public class UIBar : UI
    {
        public Color OutlineColor = Color.Gray;
        public Color EmptyColor = Color.Transparent;
        public Color FillColor = Color.Gray;
        public Action OnClick;
        public RangeValueF RangeValue = new RangeValueF(0F, 0.5F, 1F);

        public override void Click()
        {
            base.Click();
            OnClick?.Invoke();
        }

        public override void Draw(Graphics g)
        {
            var pos = GetGlobalPosition();
            if(EmptyColor != Color.Transparent)
            {
                g.FillRectangle(new SolidBrush(EmptyColor), Bounds.ToIntRect());
            }
            g.FillRectangle(new SolidBrush(FillColor), pos.x, pos.y, (RangeValue.Value == 1F ? 0F : Size.x * (1F-RangeValue.Value)), Size.y);
            g.DrawRectangle(new Pen(OutlineColor, 1F), Bounds.ToIntRect());
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
