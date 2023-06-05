using PishConverter.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PishConverter.Common.MapSpec
{
    internal class Map
    {
        public static Map m_Instance = null;
        public static Map Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Map();
                return m_Instance;
            }
        }
        public List<Drawable> Entities = new List<Drawable>();
        
        public void Inisitalize()
        {
            KB.Init();

            new User();
        }

        public void Update()
        {
            var list = new List<Drawable>(Entities);
            foreach (var e in list)
            {
                e.Update();
                if (e.Exists == false)
                    Entities.Remove(e);
            }

            KB.Update();




            if (Global.RND.Next(200) == 0)
            {
                int x = Global.RND.Next(14, Global.W - 15);
                new Shpe(0, new Vector2(x - 10, -10), new StatsInfo() { HP = 1, MoveSpeed = 0.4F });
                new Shpe(0, new Vector2(x + 10, -10), new StatsInfo() { HP = 1, MoveSpeed = 0.4F });
                new Shpe(1, new Vector2(x, -10), new StatsInfo() { HP = 4, MoveSpeed = 0.15F });
            }




            if (Global.RND.Next(10) == 0)
            {
                new Particle();
            }
        }

        public void Draw()
        {
            var list = new List<Drawable>(Entities);
            foreach (var e in list)
                e.Draw();
        }
    }
}
