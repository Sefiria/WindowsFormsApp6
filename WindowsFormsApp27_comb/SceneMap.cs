using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using Tooling.UI;

namespace WindowsFormsApp27_comb
{
    internal class SceneMap : Scene
    {
        List<UI> UI;

        public override void Initialize()
        {
            //UI = new List<UI>
            //{
            //    new UIBitmapButton(Resources.Tex);
            //};
        }
        public override void Update()
        {
        }
        public override void Draw(Graphics g)
        {
        }
        public override void DrawUI(Graphics g)
        {
            var ms = MouseStates.Position;
            g.DrawLine(new Pen(Color.White, 2F), ms, ms.Plus(24));
        }
    }
}
