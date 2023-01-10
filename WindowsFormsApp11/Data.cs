using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Bullets;
using WindowsFormsApp11.Travelers;

namespace WindowsFormsApp11
{
    public class Data
    {
        public Ship Ship;
        public List<Entity> Entities;
        public List<Traveler> Travelers => Entities.OfType<Traveler>().ToList();
        public List<Bullet> Bullets => Entities.OfType<Bullet>().ToList();

        public Data()
        {
            Ship = new Ship();
            Entities = new List<Entity>();
        }

        public void Update()
        {
            Update(Entities);

            Ship.Update();

        }

        public void Draw()
        {
            Draw(Entities);

            Ship.Draw();

        }

        private static List<T> NewList<T>(List<T> source)
        {
            return new List<T>(source);
        }
        private static void Update<T>(List<T> source)
        {
            var listT = NewList(source);
            foreach (dynamic t in listT)
                t.Update();
        }
        private static void Draw<T>(List<T> source)
        {
            var listT = NewList(source);
            foreach (dynamic t in listT)
                t.Draw();
        }
    }
}
