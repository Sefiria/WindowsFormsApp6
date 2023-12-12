using System;
using System.Collections.Generic;
using System.Drawing;
using Tooling;
using WindowsFormsApp17.items;

namespace WindowsFormsApp17
{
    internal class fluid : Circle
    {
        public bool Destroy = false;

        //public Angle angle;
        //public float force = 0F;
        vecf m_look = vecf.Zero;
        public vecf look
        {
            get => m_look;
            set
            {
                m_look = value;
                if (m_look.x < -AmplitudeMax) m_look.x = -AmplitudeMax;
                if (m_look.x > AmplitudeMax) m_look.x = AmplitudeMax;
                if (m_look.y < -AmplitudeMax) m_look.y = -AmplitudeMax;
                if (m_look.y > AmplitudeMax) m_look.y = AmplitudeMax;
                if (m_look.x < OPTI_ValueConsiderDisabled && m_look.y < OPTI_ValueConsiderDisabled)
                {
                    OPTI_TicksBeforeDisabled++;
                }
                else
                {
                    OPTI_TicksBeforeDisabled = 0;
                    OPTI_disabled = false;
                }
            }
        }
        float AmplitudeMax = 8F;
        int ticksSinceLastHit = 0, ticksSinceLastHitMax = 100;
        int OPTI_TicksBeforeDisabled = 0;
        float OPTI_ValueConsiderDisabled = 0.5F;
        bool OPTI_disabled = false;

        public fluid(float x, float y, float r) : base(x, y, r)
        {
            //angle = new Angle(Angle.Down);
        }

        public void Update(List<fluid> fluids, List<mur> murs)
        {
            if (OPTI_TicksBeforeDisabled >= 5 && (OPTI_TicksBeforeDisabled > 50 || ticksSinceLastHit >= ticksSinceLastHitMax))
            {
                OPTI_TicksBeforeDisabled = 0;
                OPTI_disabled = true;
            }
            if (OPTI_disabled) return;

            OPTI_TicksBeforeDisabled++;

            look.y += (Maths.g / 10F) * Math.Max(0.2F, ticksSinceLastHit / (float)ticksSinceLastHitMax);

            int collision = 0;
            float absorption = 0.5F;

            foreach (var fluid in fluids)
            {
                if (Destroy)
                    break;
                if(fluid.Destroy)
                    continue;

                if (CollidesFluid(vec, vec + look, fluid))
                {
                    collision = -1;
                    var v = new vecf(fluid.vec - vec).Normalized() * (-(1F-absorption*(1F - ticksSinceLastHit / (float)ticksSinceLastHitMax)));
                    ticksSinceLastHit = 0;

                    if (Maths.Abs(v.x) < OPTI_ValueConsiderDisabled && Maths.Abs(v.y) < OPTI_ValueConsiderDisabled)
                    {
                        //if (r >= fluid.r)
                        //{
                        //    Data.Instance.fluidmgr.fluids.Find(f => f == fluid).Destroy = true;
                        //    r += fluid.r / 4F;
                        //}
                        //else
                        //{
                        //    Destroy = true;
                        //    fluid.r += r / 4F;
                        //    return;
                        //}
                        continue;
                    }

                    look += v;
                    fluid.look += v * (-1F);
                }
            }

            foreach (var mur in murs)
            {
                collision = CollidesMur(vec, vec + look, mur);
                if (collision != 0)
                    break;
                collision = 0;
            }
            if(collision != 0)
            {
                /// 1 : corner top-left 
                /// 2 : corner bottom-left 
                /// 3 : corner top-right 
                /// 4 : corner bottom-right 
                /// 5 : one inside the other 
                /// 6 : segment top 
                /// 7 : segment right 
                /// 8 : segment bottom 
                /// 9 : segment left 
                switch (collision)
                {
                    case 1:
                        if (look.x.IsPositive()) look.x *= -(1F - absorption);
                        if (look.y.IsPositive()) look.y *= -(1F - absorption);
                        break;
                    case 2:
                        if (look.x.IsPositive()) look.x *= -(1F - absorption);
                        if (look.y.IsNegative()) look.y *= -(1F - absorption);
                        break;
                    case 3:
                        if (look.x.IsNegative()) look.x *= -(1F - absorption);
                        if (look.y.IsPositive()) look.y *= -(1F - absorption);
                        break;
                    case 4:
                        if (look.x.IsNegative()) look.x *= -(1F - absorption);
                        if (look.y.IsNegative()) look.y *= -(1F - absorption);
                        break;
                    case 5:
                        look.x *= -(1F - absorption);
                        look.y *= -(1F - absorption);
                        break;
                    case 6:
                        if (look.y.IsPositive()) look.y *= -(1F - absorption);
                        break;
                    case 7:
                        if (look.x.IsNegative()) look.x *= -(1F - absorption);
                        break;
                    case 8:
                        if (look.y.IsNegative()) look.y *= -(1F - absorption);
                        break;
                    case 9:
                        if (look.x.IsPositive()) look.x *= -(1F - absorption);
                        break;
                }

                ticksSinceLastHit = 0;
            }
            else
            {
                if(ticksSinceLastHit < ticksSinceLastHitMax) ticksSinceLastHit++;
            }

            int n_apr_virgule = 1;
            vec.x += (float)Math.Round(look.x, n_apr_virgule);
            vec.y += (float)Math.Round(look.y, n_apr_virgule);

            look *= 0.95F;
            if (Math.Round(look.x, n_apr_virgule) == 0) look.x = 0F;
            if (Math.Round(look.y, n_apr_virgule) == 0) look.y = 0F;
        }
        internal int CollidesMur(vecf a, vecf b, mur mur)
        {
            vecf loc = new vecf(vec);
            int collision = 0;
            float movespeed = 1F;
            for (double t = 0D; t <= 1D && collision == 0; t += 1D / (movespeed * 2F))
            {
                loc.x = Maths.Lerp(a.x, b.x, t);
                loc.y = Maths.Lerp(a.y, b.y, t);
                collision = Maths.CollisionCercleBox(new Circle(this) { vec = loc }, mur);
            }
            return collision;
        }
        internal bool CollidesFluid(vecf a, vecf b, fluid fluid)
        {
            vecf loc = new vecf(vec);
            bool collision = false;
            float movespeed = 1F;
            for (double t = 0D; t <= 1D && !collision; t += 1D / (movespeed * 2F))
            {
                loc.x = Maths.Lerp(a.x, b.x, t);
                loc.y = Maths.Lerp(a.y, b.y, t);
                collision = Maths.CollisionCercleCercle(new Circle(this) { vec = loc }, fluid);
            }
            return collision;
        }

        public void Display()
        {
            var disprect = new Rectangle(rect.Location, rect.Size);
            disprect.Inflate((int)r / 2, (int)r / 2);
            float v = Math.Max(Maths.Abs(look.x) ,Maths.Abs(look.y)) / (AmplitudeMax / 2);
            if (v > 1F) v = 1F;
            Color c = Color.FromArgb((byte)(255 * v), 64 + (byte)(191 * v), 128 + (byte)(127 * v));
            Core.g.FillEllipse(OPTI_disabled ? Brushes.Gray : new SolidBrush(c), disprect);
        }
    }
}
