using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.Entities.Enumerations;

namespace WindowsFormsApp1
{
    public class Marchandise
    {
        public string Nom = "Unknown";
        public long Prix = 0;
        public MarchandiseStructure Structure;
        public List<Bitmap> Images = new List<Bitmap>();

        public Marchandise(string Nom, long Prix, MarchandiseStructure Structure, List<Bitmap> Images)
        {
            this.Nom = Nom;
            this.Prix = Prix;
            this.Structure = Structure;
            this.Images = Images;
            foreach (var img in Images)
                img.MakeTransparent(Color.White);
        }
    }
}
