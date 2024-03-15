using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp26.Properties;

namespace WindowsFormsApp26.Entities.Structures
{
    internal class Transformator : Entity
    {
        public static new Size Size = new Size(Resources.Transformator.Width, Resources.Transformator.Height);

        public Transformator() : base()
        {
            Initialize();
        }
        public Transformator(Point position) : base(position)
        {
            Initialize();
        }
        private void Initialize()
        {
            IsKinetic = true;
            Image = Resources.Transformator;
            Image.MakeTransparent(Color.White);
            Collider = new Collider(ID, Resources.TransformatorCLD);
        }

        public override void Update()
        {
            base.Update();
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
    }
}
