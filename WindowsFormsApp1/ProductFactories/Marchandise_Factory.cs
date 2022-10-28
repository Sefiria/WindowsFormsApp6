using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entities;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1.StandFactories
{
    public static class Marchandise_Factory
    {
        public static Marchandise Create(string name)
        {
            return new Marchandise(
                    Nom: name,
                    Prix: 1,
                    Structure: Enumerations.MarchandiseStructure._1x1,
                    Images: new List<Bitmap>() { (Bitmap)Resources.ResourceManager.GetObject(name) }
            );
        }
        public static Marchandise Create(Marchandise @base)
        {
            return new Marchandise(
                    Nom: @base.Nom,
                    Prix: @base.Prix,
                    Structure: @base.Structure,
                    Images: @base.Images);
        }
    }
}
