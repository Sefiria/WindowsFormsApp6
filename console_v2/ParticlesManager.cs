using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    internal class ParticlesManager
    {
        public static List<Particle> Particles = new List<Particle>();
        public static void Update()
        {
            var particles = new List<Particle>(Particles);
            particles.ForEach(p => { if (p.Exists == false) Particles.Remove(p); else p.Update(); });
        }
        public static void Draw(Graphics g)
        {
            var particles = new List<Particle>(Particles);
            particles.ForEach(e => { if (e.Exists) e.Draw(g); });
        }

        public static void Generate(vecf position, float size_around, float force, Color color, int count_around, int duration_around)
        {
            int rnd = RandomThings.rnd(count_around / 4, count_around / 2);
            int count = RandomThings.rnd(count_around - rnd, count_around + rnd);
            float rndf = RandomThings.rnd(size_around / 4, size_around / 2);
            float size = RandomThings.rnd(size_around - rndf, size_around + rndf);
            rnd = RandomThings.rnd(duration_around / 4, duration_around / 2);
            int duration = RandomThings.rnd(duration_around - rnd, duration_around + rnd);
            for (int i=0;i<count;i++)
                Particles.Add(new Particle(position, size, (RandomThings.rnd1Around0(), -RandomThings.rnd1()).Vf(), force, color.ToArgb(), duration));
        }
    }
}
