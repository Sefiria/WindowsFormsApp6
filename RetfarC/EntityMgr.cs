using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public class EntityMgr
    {
        public static List<Entity> Entities = new List<Entity>();

        public static void Update()
        {
            var list = new List<Entity>(Entities);
            foreach (var e in list)
            {
                e.Update();
                if (e.ToVisible)
                {
                    e.Visible = true;
                    e.ToVisible = false;
                }
                if (e.Destroy)
                    Entities.Remove(e);
            }
        }

        public static void Draw()
        {
            Entities.ForEach(e => e.Draw());
        }

        public static List<Entity> GetEntitiesAt(VecF pos)
        {
            return GetEntitiesAt((int)pos.X, (int)pos.Y);
        }
        public static List<Entity> GetEntitiesAt(int x, int y)
        {
            return Entities.Where(e => x == (int)e.Pos.X && y == (int)e.Pos.Y).ToList();
        }

        public static void DiffuseClick(MouseEventArgs e)
        {
            Entities.ForEach(ent => ent.Click(e));
        }
    }
}
