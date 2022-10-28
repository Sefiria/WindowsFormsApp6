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
    public static class Marchandise_Ble_Factory
    {
        public static Marchandise Create()
        {
            return new Marchandise(
                    Nom: "Blé",
                    Prix: 1,
                    Structure: Enumerations.MarchandiseStructure._1x1,
                    Images: new List<Bitmap>() { Resources.ble });
        }
    }
}
