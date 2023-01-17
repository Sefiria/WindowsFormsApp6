using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class ManageEntities
    {
        public static Boundary MainBoundary = new Boundary();
        public static List<Boundary> Boundaries = new List<Boundary>();

        public static void Reset()
        {
            MainBoundary = new Boundary();
            Boundaries = new List<Boundary>();
        }

        public static void Update()
        {
            MainBoundary.Update();
            foreach(Boundary boundary in Boundaries)
                boundary.Update();
        }
        public static void Draw()
        {
            MainBoundary.Draw();
            foreach (Boundary boundary in Boundaries)
                boundary.Draw();
        }
    }
}
