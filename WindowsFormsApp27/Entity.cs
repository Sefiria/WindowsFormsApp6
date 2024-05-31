using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp27.Properties;
using Tooling;
using System.Linq;
using WindowsFormsApp27.entities;
using System.Text;

namespace WindowsFormsApp27
{
    public class Entity
    {
        public static List<Entity> Entities = new List<Entity>();
        public static List<People> Peoples => Entities.OfType<People>().Where(x => x.exists).ToList();
        public static List<Fruit> Fruits => Entities.OfType<Fruit>().Where(x => x.exists).ToList();

        public Guid ID = Guid.NewGuid();
        public bool exists = true;
        public int type, w, h;
        public float x, y;
        public vecf xy => (x, y).Vf();
        public vecf xy_old;
        public string dna;
        public Bitmap Image;

        public static Entity Create(int type, float x, float y, string dna)
        {
            var e = new Entity();

            e.type = type;
            e.x = x;
            e.y = y;

            e.Image = e.GetImage();
            e.w = e.Image.Width; e.h = e.Image.Height;

            e.ApplyDna(dna);

            Entities.Add(e);
            return e;
        }
        protected virtual void ApplyDna(string dna)
        {
            this.dna = dna;
        }

        public virtual Bitmap GetImage() { return null; }

        public virtual void Draw(Graphics g)
        {
            g.DrawImage(Image, x - w / 2, y - h / 2);
        }

        public void PreUpdate()
        {
        }
        public virtual void Update()
        {
        }
        public void PostUpdate()
        {
        }

        public static List<xdna> GetDNAInfos(string dna)
        {
            List<xdna> xdnas = new List<xdna>();
            int offset = 0;
            string _s;
            int _i;
            byte _b;
            xdna _xdna = new xdna();
            bool eof() => offset >= dna.Length / 2;
            while (!eof())
            {
                _s = dna.Substring(offset, 2);
                _i = int.Parse(_s);
                _b = (byte)_i;
                if (offset % 2 == 0)
                {
                    _xdna.maj = _b;
                }
                else
                {
                    _xdna.min = _b;
                    _xdna = new xdna();
                }
                offset++;
            }
            return xdnas;
        }
        public static byte GetDNAMinorFromMajor(string dna, byte major) => GetDNAMinorFromMajor(dna, major, 1)[0];
        public static byte[] GetDNAMinorFromMajor(string dna, byte major, int length)
        {
            byte[] result = new byte[length];
            for(int i=0;i<length;i++)
                result[i] = Convert.ToByte(dna.Substring(dna.IndexOf(major.ToString("X2")) + 2 + 2 * i, 2), 16);
            return result;
        }
        public static void SetDNAMinorFromMajor(ref string dna, byte major, int length, params byte[] minor)
        {
            int index = dna.IndexOf(major.ToString("X2"));
            if (index == -1)
                return;

            StringBuilder sb = new StringBuilder(dna);
            for (int i = 0; i < Math.Min(minor.Length, length); i++)
            {
                sb.Remove(index + 2 + 2 * i, 2);
                sb.Insert(index + 2 + 2 * i, minor[i].ToString("X2"));
            }

            dna = sb.ToString();
        }

        public static void MuteDNAMajor(ref string dna, byte major, int length, float amplitude)
        {
            byte[] minor = GetDNAMinorFromMajor(dna, major, length);
            for (int i = 0; i < length; i++)
                minor[i] += (byte)(RandomThings.rnd1Around0() * amplitude).ByteCut();
            SetDNAMinorFromMajor(ref dna, major, length, minor);
        }

        public virtual string dna_muted(){ return dna; }
    }
}
