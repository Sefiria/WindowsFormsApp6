using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp24
{
    internal class UIMouseAssist
    {
        internal static int TilesMaxDistance = 5;
        internal static List<PointF> Path = new List<PointF>();

        static UIMouseAssist()
        {
        }

        internal static void Update()
        {
            var ts = Core.TileSize;
            var ms = MouseStates.Position;
            var p = Core.MainCharacter;
            var ev = Map.Current.Events.Where(e => Maths.Distance(e.Position, p.Position) < TilesMaxDistance * ts).ToList().FirstOrDefault(e => e.Bounds.Contains(ms));
            bool evIsHand = false;
            if (ev == null && p.HandObject > -1)
            {
                ev = Map.Current.Events[p.HandObject];
                evIsHand = true;
            }
            if (ev != null)
            {
                if(!evIsHand)
                    ev.MouseHover = true;
                var primary = Core.GetInput(Enumerations.InputNames.Primary, true);
                var secondary = Core.GetInput(Enumerations.InputNames.Secondary, true);
                var primaryDown = Core.GetInput(Enumerations.InputNames.Primary);
                var secondaryDown = Core.GetInput(Enumerations.InputNames.Secondary);
                if (primary || secondary)
                {
                    ev.Data["X"] = (Path.Last().X - ts / 2).SnapF(ts);
                    ev.Data["Y"] = (Path.Last().Y - ts / 2).SnapF(ts);
                    if(primary) ev.PrimaryAction();
                    else if(primaryDown) ev.PrimaryActionDown();
                    else if(secondary) ev.SecondaryAction();
                    else if(secondaryDown) ev.SecondaryActionDown();
                }
            }
        }

        internal static void Draw()
        {
            var max_distance = Core.MainCharacter.HandObject == -1 ? TilesMaxDistance : 3;

            var g = Core.Instance.g;
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;
            var p = scene.MainCharacter;
            var ts = Core.TileSize;

            var pen = new Pen(Color.FromArgb(100, Color.White), 2F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Path = new List<PointF>();
            PointF ms = MouseStates.Position;
            float d = Maths.Distance(p.Position, ms);
            for (float t = 0F; t <= 1F; t += 1F / d)
            {
                PointF tile = Maths.Lerp(p.Position, ms, t).Snap(ts).PlusF(ts / 2, ts / 2F);
                if (!Path.Contains(tile))
                {
                    if(Path.Count > 1 && Maths.Distance(Path[Path.Count-2], tile) < ts * 1.5F)
                        Path.Remove(Path.Last());
                    Path.Add(tile);
                }
                if (Path.Count == max_distance)
                    break;
            }

            if (Path.Count > 1)
            {
                Core.Instance.g.FillRectangle(new SolidBrush(Color.FromArgb(50, 100, 200, 255)), Path.Last().X - ts / 2, Path.Last().Y - ts / 2, Core.TileSize, Core.TileSize);
                g.DrawLines(pen, Path.ToArray());
            }
        }
    }
}
