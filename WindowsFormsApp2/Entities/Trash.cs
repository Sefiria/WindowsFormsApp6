using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Entities
{
    public class Trash : DrawableEntity
    {
        public Trash(int TX = 0, int TY = 0) : base(TX * SharedCore.TileSize, TY * SharedCore.TileSize)
        {
        }

        public override void Draw()
        {
            //SharedCore.g.DrawImage(Resources.trash, X, Y);
        }

        public override void Update()
        {
        }
    }
}
