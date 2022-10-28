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
    public static class Stand_Factory
    {
        public static Stand Create(string productName, int TX, int TY)
        {
            Marchandise marchandise = new Marchandise(
                    Nom: productName,
                    Prix: 1,
                    Structure: Enumerations.MarchandiseStructure._2x1,
                    Images: new List<Bitmap>() { (Bitmap)Resources.ResourceManager.GetObject(productName) });
            List<Slot> slots = new List<Slot>()
            {
                new Slot(TX,   TY, 14, 9),
                new Slot(TX,   TY, 34, 9),
                new Slot(TX+1, TY, 14, 9),
                new Slot(TX+1, TY, 34, 9),
                new Slot(TX,   TY, 18, 21),
                new Slot(TX,   TY, 38, 21),
                new Slot(TX+1, TY, 18, 21),
                new Slot(TX+1, TY, 38, 21),
                new Slot(TX,   TY, 22, 33),
                new Slot(TX,   TY, 42, 33),
                new Slot(TX+1, TY, 22, 33),
                new Slot(TX+1, TY, 42, 33),
            };
            Stand stand = new Stand($"Stand de {productName}", marchandise, slots, TX, TY);
            return stand;
        }
    }
}
