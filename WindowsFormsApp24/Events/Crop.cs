using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Tooling;
using Tooling.UI;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Crop : Event
    {
        internal Crop(NamedObjects namedObject, float x, float y) : base(Core.NamedTextures[namedObject], true, x, y)
        {
            Object = namedObject;
            start_grow_tick = Core.Instance.Ticks;
            ImageSource = (Bitmap)Image.Clone();
            Update();
        }

        //internal static Bitmap dirt_behind, dirt_above;

        internal NamedObjects Object;
        internal long start_grow_tick, total_time_to_grow = 0;
        internal Bitmap ImageSource;
        internal List<Grass> Grasses = new List<Grass>();
        internal bool FinishedToGrow = false;

        internal long remaining_ticks => Core.Instance.Ticks > start_grow_tick + total_time_to_grow ? 0 : start_grow_tick + total_time_to_grow - Core.Instance.Ticks;
        internal bool IsReady => remaining_ticks == 0;

        internal void GenImage()
        {
            if (IsReady && total_time_to_grow > 0)
            {
                if (!FinishedToGrow)
                {
                    var howmanyseeds = RandomThings.rnd(3);
                    for (int i = 0; i < howmanyseeds; i++)
                        Map.Current.Events.Add(new Seed(Object, total_time_to_grow, X + RandomThings.arnd(-8, 8), Y + RandomThings.arnd(-8, 8)));
                    FinishedToGrow = true;
                }
                Image = ImageSource;
            }
            else
            {
                Image = new Bitmap(Image.Width, Image.Height);
                using (Graphics g = Graphics.FromImage(Image))
                    g.DrawImage(ImageSource, 0, (int)(Core.TileSize * (total_time_to_grow == 0 ? 1F : (remaining_ticks / (float)total_time_to_grow))));
            }
        }

        internal override void Update()
        {
            MouseHover = false;

            if (Map.Current.Tiles[Map.Current.Tiles.PointedLayer(TileX, TileY), TileX, TileY].wet == 0F)
                start_grow_tick++;

            Layer = remaining_ticks == 0 ? EvLayer.Same : EvLayer.Below;
            GenImage();
            Grasses.ForEach(grass => grass.Update());
        }

        internal override void Draw()
        {
            if (!IsReady)
            {
                MouseHover = false;
            }
            base.Draw();

            if (Character.MainHandObjectDefined && IsOnScreen && Character.MainHandEvent is WateringCan)
                DrawExtraInfos();
        }

        internal override void DrawExtraInfos()
        {
            var cam = Core.Cam;
            var diameter = 16F;
            var radius = diameter / 2F;
            var wet = Map.Current.Tiles[Map.Current.Tiles.PointedLayer(TileX, TileY)/*Core.MainCharacter.LayerTile*/, TileX, TileY].wet;
            Core.Instance.gUI.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.Black)), X - cam.X + radius, Y - cam.Y + radius, diameter, diameter);
            Core.Instance.gUI.FillEllipse(wet >= 0.5F ? new SolidBrush(Color.FromArgb(100, Color.DeepSkyBlue)) : (wet >= 0.25F ? new SolidBrush(Color.FromArgb(100, Color.Orange)) : new SolidBrush(Color.FromArgb(100, Color.Red))), X - cam.X + radius + radius * (1F-wet), Y - cam.Y + radius + radius * (1F - wet), diameter * wet, diameter * wet);
            if (wet < 1F)
                Core.Instance.gUI.DrawEllipse(Pens.Black, X - cam.X + radius, Y - cam.Y + radius, diameter, diameter);
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
