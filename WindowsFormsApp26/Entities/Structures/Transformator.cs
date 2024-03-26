using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp26.Properties;

namespace WindowsFormsApp26.Entities.Structures
{
    internal class Transformator : Entity
    {
        public const int ArmTop = 76;
        public float ConveyorSpeed = 0.2F;

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
            ForegroundAreas.Add(Collider.LocalBoxes[0]);
        }

        public override void Update()
        {
            base.Update();
            MoveEntitiesAbove();
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }


        private void MoveEntitiesAbove()
        {
            int arm_margin = 8;
            Core.CurrentEntities.Except(new List<Entity> { this })
                                           .Where(e => e.Right >= Left && e.Left < Right && e.Bottom < Y + ArmTop + arm_margin && e.Bottom > Y + ArmTop - arm_margin)
                                           .ToList()
                                           .ForEach(e => e.X += ConveyorSpeed);
        }
    }
}
