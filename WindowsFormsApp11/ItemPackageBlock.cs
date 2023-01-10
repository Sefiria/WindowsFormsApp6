using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public class ItemPackageBlock : Entity
    {
        public bool Destroy = false;
        public ItemPackage Package;
        public int Size;
        public float hSize => Size / 2F;
        public override Rectangle Rect => new Rectangle((int)(X - hSize), (int)(Y - hSize), Size, Size);

        public ItemPackageBlock(float x, float y, ItemPackage package)
        {
            X = x;
            Y = y;
            Package = package;

            if (Package.Count == 1) Size = 2;
            else if (Package.Count < 5) Size = 5;
            else if (Package.Count < 10) Size = 10;
            else Size = Package.Count;

            Var.Data.Entities.Add(this);
        }

        public override void Update()
        {
            Y += 4F;

            if (Maths.IsColliding(Var.Data.Ship, this))
            {
                Var.Data.Ship.AddLoot(Package);
                Destroy = true;
            }

            if (Y - hSize > Var.H)
                Destroy = true;

            if (Destroy)
                Var.Data.Entities.Remove(this);
        }

        public override void Draw()
        {
            Var.g.FillRectangle(Brushes.Yellow, X - hSize, Y - hSize, Size, Size);
        }
    }
}
