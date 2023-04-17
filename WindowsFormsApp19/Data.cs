using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp19.Utilities;

namespace WindowsFormsApp19
{
    internal class Data
    {
        private static Data m_Instance = null;
        public static Data Instance
        {
            get
            {
                if(m_Instance == null)
                    m_Instance = new Data();
                return m_Instance;
            }
        }

        public Map map;
        public vecf cam;
        //public List<car> cars;

        public void Init()
        {
            map = new Map(/*Resources.tileset_static*/);
            //cars = new List<car>();
            //cars.Add(new car(50, 50, 10, 20));
            //cars.Add(new car(150, 50, 10, 20));
        }
    }

    public class Mur
    {
        public vecf A { get; set; }
        public vecf B { get; set; }
        public vecf Normale { get; set; } = vecf.Zero;
        public Mur(vecf a, vecf b)
        {
            A = a;
            B = b;
            Normale = Maths.Normale(A, B);
        }
    }
    public class Map
    {
        public List<Mur> murs { get; set; } = new List<Mur>();

        public Map()
        {
        }

        public void Draw()
        {
            foreach (var mur in murs)
                Core.g.DrawLine(new Pen(Color.FromArgb(Core.RND.Next(256), Core.RND.Next(256), Core.RND.Next(256)), 4F), mur.A.pt, mur.B.pt);
        }
    }
    public class JsonData
    {
        public Map map { get; set; } = new Map();
        public JsonData()
        {
        }
    }
}
