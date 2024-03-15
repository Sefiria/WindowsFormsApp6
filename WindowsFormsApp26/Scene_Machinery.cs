using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp26.Entities.Structures;

namespace WindowsFormsApp26
{
    public class Scene_Machinery : Scene
    {
        public Scene_Machinery() : base()
        {
        }
        public override void Initialize()
        {
            BGColor = Color.FromArgb(172, 167, 226);
            var transformator = new Transformator(((Point)Core.Instance.Canvas.Size).Minus((Point)Transformator.Size).Div(2));
            var drawerInput = new DrawerInput(transformator.Position.MinusF(-32f, 128f), new Size(64, 64));
        }

        public override void Update()
        {
            base.Update();
            var entities = new List<Entity>(Entities);
            entities.ForEach(e => { if (e.Exists == false) Entities.Remove(e); else e.Update(); });
            Collide(ref Entities);
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            var entities = new List<Entity>(Entities);
            entities.ForEach(e => { if (e.Exists) e.Draw(g); });
        }
    }
}
