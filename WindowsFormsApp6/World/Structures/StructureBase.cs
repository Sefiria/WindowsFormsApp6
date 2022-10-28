using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World.Structures
{
    public abstract class StructureBase : IStructure
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        [JsonIgnore] public Bitmap Image { get; set; }

        public StructureBase(int x, int y, int w, int h, Bitmap image)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Image = image;
        }

        public virtual void Draw(Bitmap image, int tilesz)
        {
            var img = Image.Resized(Image.Width * (tilesz / Core.TileSzBase), Image.Height * (tilesz / Core.TileSzBase));
            using (Graphics g = Graphics.FromImage(image))
                g.DrawImage(img, X * tilesz, Y * tilesz);
        }
        public virtual void Draw()
        {
            Core.g.DrawImage(Image, X * Core.TileSz, Y * Core.TileSz);
        }

        public virtual void Update() { }

        public abstract void MouseDown();

        public void RemoveStructureFromAllBlocks()
        {
            if (Data.Instance.State != Data.Instance.World)
                return;

            for (int x = X; x < X + W; x++)
            {
                for (int y = Y; y < Y + H; y++)
                {
                    Data.Instance.World.Blocs[x, y].OwnerStructure = null;
                }
            }
        }
        public void AddStructureToAllBlocks()
        {
            if (Data.Instance.State != Data.Instance.World)
                return;

            for (int x = X; x < X + W; x++)
            {
                for (int y = Y; y < Y + H; y++)
                {
                    Data.Instance.World.Blocs[x, y].OwnerStructure = this;
                }
            }
        }
    }
}
