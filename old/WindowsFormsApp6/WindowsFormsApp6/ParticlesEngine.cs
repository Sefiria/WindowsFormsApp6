using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public class ParticlesEngine
    {
        public class Particle
        {
            /*
                X : Location X
                Y : Location Y
                V : Velocity
                S : Size
                LX : Look X
                LY : Look Y
                R,G,B : Color
                L : Lifetime
            */
            public float InitX, InitY, X, Y, V, S, LX, LY;
            byte R, G, B;
            int L, InitL;
            public bool Dead = false;
            Color C;
            int shape;
            int IncreasingOffsetX = 0;

            public Particle(float X = 0F, float Y = 0F, float LX = 0F, float LY = 0F, float V = 0F, float S = 0F, int L = 25, byte R = 255, byte G = 255, byte B = 255)
            {
                InitX = X;
                InitY = Y;
                this.X = X;
                this.Y = Y;
                this.LX = LX;
                this.LY = LY;
                this.V = V;
                this.S = S;
                InitL = this.L = L == 0 ? 25 : L;
                this.R = R;
                this.G = G;
                this.B = B;
                C = Color.FromArgb(255, R, G, B);

                Random RND = new Random(Guid.NewGuid().GetHashCode());
                shape = RND.Next(2);
            }

            public void Draw(Graphics g, int OffsetX = 0, int OffsetY = 0)
            {
                if (IncreasingOffsetX != 0)
                {
                    X = InitX;
                    Y = InitY;
                    OffsetX += (int)(IncreasingOffsetX * (1F - L / (float)InitL));
                }

                if (shape == 0)
                    g.FillRectangle(new SolidBrush(C), OffsetX + X * Grid.BlockSize + Grid.BlockSize / 4, OffsetY + Y * Grid.BlockSize + Grid.BlockSize / 4, S, S);
                else if (shape == 1)
                    g.FillEllipse(new SolidBrush(C), OffsetX + X * Grid.BlockSize + Grid.BlockSize / 4, OffsetY + Y * Grid.BlockSize + Grid.BlockSize / 4, S, S);
            }
            public void NextStep()
            {
                if(L == 0)
                {
                    Dead = true;
                    return;
                }
                
                X += LX * V;
                Y += LY * V;
                L--;
                if (L < 0)
                    L = 0;

                int brightFreq = 30;
                int R = L % brightFreq == 0 ? 255 : this.R;
                int G = L % brightFreq == 0 ? 255 : this.G;
                int B = L % brightFreq == 0 ? 255 : this.B;
                C = Color.FromArgb((byte)((L / (float)InitL) * 255), R, G, B);
            }

            internal void AddIncreasingOffset(int offsetX)
            {
                L = InitL;
                IncreasingOffsetX = offsetX;
            }
        }

        private List<Particle> Particles = new List<Particle>();
        public void Generate(int X, int Y, int Count = 20, byte R = 255, byte G = 255, byte B = 255)
        {
            Random RND = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < Count; i++)
            {
                Particles.Add(new Particle(X, Y, RND.Next(100) / 100F - 0.5F, RND.Next(100) / 100F - 0.5F, RND.Next(100) / 200F, RND.Next(1000) / 100F, RND.Next(20000) / 100, R, G, B));
            }
        }
        public void UpdateDraw(Graphics g, int OffsetX = 0, int OffsetY = 0)
        {
            var list = new List<Particle>(Particles);
            foreach(Particle p in list)
            {
                p.Draw(g, OffsetX, OffsetY);
                p.NextStep();
                if (p.Dead)
                    Particles.Remove(p);
            }
        }
        public void Flush()
        {
            Particles.Clear();
        }

        public void AddIncreasingOffsetToParticles(int ParticleInitX, int ParticleInitY, int OffsetX)
        {
            List<Particle> list = new List<Particle>(Particles);
            foreach (Particle p in list)
                if (p != null && p.InitX == ParticleInitX && p.InitY == ParticleInitY)
                    p.AddIncreasingOffset(OffsetX);
        }
    }
}
