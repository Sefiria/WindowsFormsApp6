using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp27.Properties;

namespace WindowsFormsApp27.entities
{
    public class Fruit : Entity
    {
        float HealthState = 1F;//0 - bad ; 1 : good

        public static Fruit Create(vec v, string dna) => Create(v.x, v.y, dna);
        public static Fruit Create(float x, float y, string dna)
        {
            var e = new Fruit();

            e.type = 3;
            e.x = x;
            e.y = y;

            e.Image = Resources.fruit.Transparent();
            e.w = e.Image.Width; e.h = e.Image.Height;

            e.ApplyDna(dna);

            Entities.Add(e);
            return e;
        }
        protected override void ApplyDna(string dna)
        {
            base.ApplyDna(dna);
            byte dna_healthstate = GetDNAMinorFromMajor(dna, 0xA0);
            HealthState = dna_healthstate / (float)0xFF;
        }
    }
}
