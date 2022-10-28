using System.Drawing;
using Newtonsoft.Json;
using WindowsFormsApp6.World.Ores;
using WindowsFormsApp6.World.Structures;

namespace WindowsFormsApp6.World.Blocs
{
    public abstract class BlocBase : IBloc
    {
        public int X { get; set; }
        public int Y { get; set; }
        [JsonIgnore] public Bitmap Image { get; set; }
        public int Layer { get; set; }
        public int Life { get; set; } = 1;
        public Ore Ore { get; set; } = null;
        public IStructure OwnerStructure { get; set; } = null;

        public BlocBase(BlocGrass bloc)
        {
            X = bloc.X;
            Y = bloc.Y;
            Layer = bloc.Layer;
            Image = bloc.Image;
        }
        public BlocBase(IBloc bloc)
        {
            X = bloc.X;
            Y = bloc.Y;
            Layer = bloc.Layer;
            Image = bloc.Image;
        }
        public BlocBase(int x, int y, Bitmap image, int layer)
        {
            X = x;
            Y = y;
            Layer = layer;
            Image = image;
        }

        public virtual void Draw(Bitmap image, int tilesz)
        {
            var img = Image.Resized(tilesz);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(img, X * tilesz, Y * tilesz);
                if (Ore != null)
                {
                    int x, y, w = Ore.Image.Width, h = Ore.Image.Height, bsz = tilesz;
                    for (int i = 0; i < Ore.Count; i++)
                    {
                        x = Tools.RND.Next(bsz - w);
                        y = Tools.RND.Next(bsz - h);
                        g.DrawImage(Ore.Image, X * tilesz + x, Y * tilesz + y);
                    }
                }
                if (OwnerStructure?.X == X && OwnerStructure?.Y == Y)
                    OwnerStructure.Draw(image, tilesz);
            }
        }
        public virtual void Draw()
        {
            Core.g.DrawImage(Image, X * Core.TileSz, Y * Core.TileSz);
        }

        public virtual void Update() { }
    }
}