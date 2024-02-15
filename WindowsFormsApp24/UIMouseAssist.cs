using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using WindowsFormsApp24.Events;

namespace WindowsFormsApp24
{
    internal class UIMouseAssist
    {
        internal static int TilesMaxDistanceFar = 5, TilesMaxDistanceClose = 4;
        internal static List<PointF> Path = new List<PointF>();
        internal static Point PathTargetTile = Point.Empty;

        static UIMouseAssist()
        {
        }

        internal static void Update()
        {
            var ts = Core.TileSize;
            var ms = MouseStates.Position.DivF(Core.Cam.Zoom);
            var p = Core.MainCharacter;
            bool evIsHand = p.HandObject.IsDefined();
            Event ev = Map.Current.Events.Reverse<Event>().OrderByDescending(e => e.Z).FirstOrDefault(e => e.RealTimeDisplayArea.Contains(ms));
            if(evIsHand)
                while (ev?.AttachSource != null)
                    ev = ev.AttachSource;
            if (ev != null)
                ev.MouseHover = true;
            if (evIsHand && !(ev is EventContainer))
                ev = Map.Current.Events.First(e => e.Guid == p.HandObject);
            if (ev == null && evIsHand)
            {
                ev = Map.GetEvent(p.HandObject);
                evIsHand = true;
            }
            if(ev != null)
            {
                var cam = Core.Cam;
                var primary = Core.GetInput(Enumerations.InputNames.Primary, true);
                var secondary = Core.GetInput(Enumerations.InputNames.Secondary, true);
                var primaryDown = Core.GetInput(Enumerations.InputNames.Primary);
                var secondaryDown = Core.GetInput(Enumerations.InputNames.Secondary);
                if (primary || secondary || primaryDown || secondaryDown)
                {
                    ev.Data["X"] = (Path.Last().X + cam.X).SnapF(ts);
                    ev.Data["Y"] = (Path.Last().Y + cam.Y).SnapF(ts);
                    if(primary) ev.PrimaryAction();
                    else if(primaryDown) ev.PrimaryActionDown();
                    else if(secondary) ev.SecondaryAction();
                    else if(secondaryDown) ev.SecondaryActionDown();
                }
            }
        }

        internal static void Draw()
        {
            var max_distance = Character.MainHandObject.IsNotDefined() ? TilesMaxDistanceFar : TilesMaxDistanceClose;

            var g = Core.Instance.g;
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;
            var p = scene.MainCharacter;
            var ts = Core.TileSize;
            var cam = Core.Cam;

            var pen = new Pen(Color.FromArgb(111, Color.White), 2F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Path = new List<PointF>();
            PointF ms = MouseStates.Position.DivF(Core.Cam.Zoom).PlusF(cam.Position);
            float d = Maths.Distance(p.Position, ms);
            for (float t = 0F; t <= 1F; t += 1F / d)
            {
                var tile = Maths.Lerp(p.Position, ms, t).Snap(ts).PlusF(ts / 2F, ts / 2F).MinusF(cam.Position);
                if (!Path.Contains(tile))
                {
                    if (Path.Count > 1 && Maths.Distance(Path[Path.Count - 2], tile) < ts * 1.5F)
                        Path.Remove(Path.Last());
                    Path.Add(tile);
                }
                if (Path.Count == max_distance)
                    break;
            }
            PathTargetTile = ms.Div(ts);

            if (Maths.Distance(p.Position.Div(ts), PathTargetTile) <= max_distance)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(111, 100, 200, 255)), PathTargetTile.X * ts - cam.X, PathTargetTile.Y * ts - cam.Y, Core.TileSize, Core.TileSize);
                if (Path.Count > 1)
                    g.DrawLines(pen, Path.ToArray());
            }
            else
            {
                for (int x = p.TileX - max_distance; x < p.TileX + max_distance; x++)
                {
                    for (int y = p.TileY - max_distance; y < p.TileY + max_distance; y++)
                    {
                        if (Maths.Distance(p.Position.Div(ts), (x,y).P()) <= max_distance)
                            g.FillRectangle(new SolidBrush(Color.FromArgb(50, 100, 200, 255)), x * ts - cam.X, y * ts - cam.Y, Core.TileSize, Core.TileSize);
                    }
                }
            }
        }
    }
}
