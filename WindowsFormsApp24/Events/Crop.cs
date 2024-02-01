using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Tooling;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Crop : Event
    {
        internal Crop(string filename, float x, float y) : base(filename, x, y){ Initialize(); }
        internal Crop(Bitmap image, float x, float y) : base(image, true, x, y) { Initialize(); }
        internal Crop(Bitmap image, object unique_image_tag, float x, float y) : base(image, unique_image_tag, x, y) { Initialize(); }

        //internal static Bitmap dirt_behind, dirt_above;

        internal long start_grow_tick, total_time_to_grow;
        internal Bitmap ImageSource;
        internal List<Grass> Grasses = new List<Grass>();

        internal long remaining_ticks => Core.Instance.Ticks > start_grow_tick + total_time_to_grow ? 0 : start_grow_tick + total_time_to_grow - Core.Instance.Ticks;
        internal bool IsReady => remaining_ticks == 0;

        static Crop()
        {
            //var sz = Core.TileSize;
            //dirt_behind = new Bitmap(sz, sz);
            //dirt_above = new Bitmap(sz, sz);
            //HatchBrush hatchBrush = new HatchBrush(HatchStyle.DashedVertical, Color.Transparent, Color.Maroon);
            //var pen = new Pen(hatchBrush, 4F);
            //using (Graphics g = Graphics.FromImage(dirt_behind))
            //    g.DrawLine(pen, sz*0.05F, sz/2 - 6, sz-sz*0.05F, sz/2 - 6);
            //using (Graphics g = Graphics.FromImage(dirt_above))
            //    g.DrawLine(pen, sz*0.05F, sz/2 - 2, sz-sz*0.05F, sz/2 - 2);
        }

        internal void Initialize()
        {
            start_grow_tick = Core.Instance.Ticks;
            ImageSource = (Bitmap)Image.Clone();
        }

        private void GenImage()
        {
            if (IsReady)
                Image = ImageSource;
            else
            {
                Image = new Bitmap(Image.Width, Image.Height);
                using (Graphics g = Graphics.FromImage(Image))
                    g.DrawImage(ImageSource, 0, (int)(Core.TileSize * (remaining_ticks / (float)total_time_to_grow)));
            }
        }

        internal override void Update()
        {
            MouseHover = false;

            Layer = remaining_ticks == 0 ? EvLayer.Same : EvLayer.Below;
            GenImage();
            Grasses.ForEach(grass => grass.Update());
        }

        internal override void Draw()
        {
            if (!IsReady)
                MouseHover = false;
            //if (Map.Current.DrawingPart == DrawingPart.Bottom)
            //{
            //    var pos = Position.PlusF(Offset).Minus(Core.Cam.Position);
            //    Core.Instance.g.DrawImage(dirt_behind, pos);
            //    base.Draw();
            //    Core.Instance.g.DrawImage(dirt_above, pos);
            //}
            //else
            //{
                base.Draw();
            //}
        }

        internal override void SecondaryAction()
        {
            if (IsReady)
            {
                PredefinedActions[PredefinedAction.TakeDrop](this);
            }
        }
    }
}
