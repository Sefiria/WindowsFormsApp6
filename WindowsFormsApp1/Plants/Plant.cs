using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Entities;
using WindowsFormsApp1.Properties;
using WindowsFormsApp1.StandFactories;

namespace WindowsFormsApp1.Plants
{
    public class Plant : DrawableEntity, IDisposable
    {
        public Marchandise Marchandise;
        public int MaxProductCount;

        private int GrowStep = 0;
        private int GrowStepMax => PlantImages.Count;
        private List<Bitmap> PlantImages;

        public Plant(Marchandise Marchandise, int MaxProductCount, int TX, int TY)
        {
            Traversable = true;

            this.Marchandise = Marchandise;
            this.MaxProductCount = MaxProductCount;

            X = TX * SharedCore.TileSize;
            Y = TY * SharedCore.TileSize;

            var img = (Bitmap)Resources.ResourceManager.GetObject($"plant_{Marchandise.Nom}");
            if (img == null) throw new NullReferenceException($"No image related to {Marchandise.Nom} in resources file");

            PlantImages = Tools.SplitImage(img, 32, 48);
            Image = PlantImages[0];

            UpdatePart.TimerPlantGrow.Tick += TimerGrow_Tick;
        }

        private void TimerGrow_Tick(object sender, EventArgs e)
        {
            if (GrowStep < GrowStepMax - 1)
            {
                GrowStep++;
                Image = PlantImages[GrowStep];
            }
            if (GrowStep == GrowStepMax)
                GrowStep--;
        }

        public List<Marchandise> GetFruits()
        {
            var result = new List<Marchandise>();
            for (int i = GrowStepMax - GrowStep; i <= MaxProductCount; i++)
            {
                result.Add(Marchandise_Factory.Create(Marchandise));
                GrowStep--;
                Image = PlantImages[GrowStep];
            }
            return result;
        }

        public void Dispose()
        {
            UpdatePart.TimerPlantGrow.Tick -= TimerGrow_Tick;
        }

        public override void Update()
        {
        }
    }
}
