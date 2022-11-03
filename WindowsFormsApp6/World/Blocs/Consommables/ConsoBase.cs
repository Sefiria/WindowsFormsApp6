using Newtonsoft.Json;
using System.Drawing;
using WindowsFormsApp6.Particules;

namespace WindowsFormsApp6.World.Blocs.Consommables
{
    public class ConsoBase
    {
        public int X, Y;
        [JsonIgnore] public Bitmap Image = null, ImageUsed = null;
        public int m_Life = 1;
        public int Life
        {
            get => m_Life;
            set
            {
                if (value < m_Life && Data.Instance.State == Data.Instance.World)
                    Data.Instance.World.Particules.AddRange(Particule.RangeRND(X.ToCurWorld(), Y.ToCurWorld(), 1, ImageUsed, 100));
                m_Life = value;
                if (m_Life <= 0)
                {
                    m_Life = 0;
                    Used = true;
                    GetConsoResource();
                }
                else
                    Used = false;
            }
        }
        public bool Used = false;

        public Bitmap GetImage() => Used ? ImageUsed : Image;

        public ConsoBase(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Draw()
        {
            if (Used == false)
            {
                if (Image != null)
                    Core.g.DrawImage(Image, X * Core.TileSz, Y * Core.TileSz - (Image.Height - Core.TileSz));
            }
            else
            {
                if (ImageUsed != null)
                    Core.g.DrawImage(ImageUsed, X * Core.TileSz, Y * Core.TileSz);
            }
        }

        public virtual void GetConsoResource() { }
    }
}
