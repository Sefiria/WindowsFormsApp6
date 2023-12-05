using Cast.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cast
{
    public class RayCastTool
    {
        public static (PointF ContactPoint, Structural Structure, Entity Entity) RayCast(PointF px, PointF forward)
        {
            Entity Entity = null;
            while (Maths.Distance(px, Core.Cam.Position) < Core.Cam.Far)
            {
                px = px.PlusF(forward);
                var wall = Core.Map.Structures.FirstOrDefault(w => Maths.PointOnLine2D(px, w.A, w.B, 0.00001F));
                if (wall != null)
                    return (px, wall, Entity);
                var entity = Core.Entities.entities.FirstOrDefault(e => e.Bounds.Contains(px));
                if (entity != null)
                    Entity = entity;
            }
            return (PointF.Empty, null, Entity);
        }
        public static List<(PointF? ContactPoint, int LineX, Entity Entity)> RayCastEntities(PointF origin, PointF forward, float aA, float aB)
        {
            List<(PointF? ContactPoint, int LineX, Entity Entity)> result = new List<(PointF? ContactPoint, int LineX, Entity Entity)>();
            Entity entity = null;
            List<PointF?> pts = new List<PointF?>();
            List<PointF?> rays = new List<PointF?>();
            PointF? px;
            for (float f = aA; f <= aB; f+=3)
            {
                rays.Add(Maths.Rotate(forward, f));
                pts.Add(origin.PlusF(rays.Last().Value));
            }

            while (rays.Any(x => x != null))
            {
                List<PointF?> list = new List<PointF?>(rays);
                foreach (var ray in list)
                {
                    px = pts[list.IndexOf(ray)];
                    if (px == null) continue;
                    entity = Core.Entities.entities.FirstOrDefault(e => e.Bounds.Contains(px.Value));
                    if (entity != null)
                    {
                        result.Add((px, list.IndexOf(ray), entity));
                        rays[list.IndexOf(ray)] = null;
                        pts[list.IndexOf(ray)] = null;
                        continue;
                    }
                    if (Maths.Distance(px.Value, Core.Cam.Position) > Core.Cam.Far)
                    {
                        rays[list.IndexOf(ray)] = null;
                        pts[list.IndexOf(ray)] = null;
                        continue;
                    }
                    pts[list.IndexOf(ray)] = px.Value.PlusF(ray.Value);
                }
            }
            return result;
        }
    }
}
