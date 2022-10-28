using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities.Pentities;
using WindowsFormsApp2.Interfaces;

namespace WindowsFormsApp2.Entities
{
    public class Knife : Item, IDamager, IThrowable
    {
        public MaterialQuality Material { get; set; }
        public Knife(MaterialQuality Material, Point? Coord = null, bool Drawable = false) : base(Items.Knife, Coord, Drawable)
        {
            this.Material = Material;
        }

        public int Damage => this.GetDamage();

        public static List<Item> Repeat(MaterialQuality material, int count, Point? Coord = null, bool Drawable = false) => Enumerable.Repeat(new Knife(material, Coord, Drawable), count).Cast<Item>().ToList();

        public void Throw(DrawableEntity e)
        {
            int ts = SharedCore.TileSize;
            var x = e.TX * ts + e.Image.Width / 2 - Image.Width / 2;
            var y = e.TY * ts + e.Image.Height / 2 - Image.Height / 2;
            new Pentity(e, this, e.X, e.Y, Image, new VecF() { X = e.Look.X, Y = e.Look.Y });
        }
    }
}
