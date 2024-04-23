using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp27_comb
{
    internal class SceneGame : Scene
    {
        public override void Initialize()
        {
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
            g.DrawLine(new Pen(Color.White, 6F), ms, ms.Plus(16));
        }
    }
}
