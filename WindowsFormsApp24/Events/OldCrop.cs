using System.Collections.Generic;
using System.Drawing;
using Tooling;

namespace WindowsFormsApp24.Events
{
    internal class OldCrop : Event
    {
        internal OldCrop(float x, float y) : base(x, y){ Initialize(); }
        internal OldCrop(string filename, float x, float y) : base(filename, x, y){ Initialize(); }
        internal OldCrop(Bitmap image, float x, float y) : base(image, x, y) { Initialize(); }
        internal OldCrop(Bitmap image, object unique_image_tag, float x, float y) : base(image, unique_image_tag, x, y) { Initialize(); }

        internal Graphics g;
        internal long start_grow_tick;
        internal Root Root;
        internal List<Grass> Grasses = new List<Grass>();

        internal void Initialize()
        {
            Image = new Bitmap(Core.TileSize * 3, Core.TileSize * 3);
            Offset = (-Core.TileSize, -Core.TileSize*2).P();
            g = Graphics.FromImage(Image);
            start_grow_tick = (long)RandomThings.rnd(10, 100);
        }

        internal override void Update()
        {
            if (Root == null && Core.Instance.Ticks >= start_grow_tick)
                Root = new Root(this, null);
            Root?.Update();
            Grasses.ForEach(grass => grass.Update());

            GenerateImage();
        }

        internal override void Draw()
        {
            //Core.Instance.g.DrawImage(Map.Current.Tileset[8], Position.Minus(0, Core.TileSize / 2).Minus(Core.Cam.Position));
            base.Draw();
        }

        private void GenerateImage()
        {
            Grasses.ForEach(grass => grass.GenerateDraw(g));
            Root?.GenerateDraw(g);
        }
    }
}
