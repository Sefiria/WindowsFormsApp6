﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public static class Extensions
    {
        public static vec ToTile(this vecf position) => (position / GraphicsManager.CharSize.V()).i;
        public static vecf ToWorld(this vec tile) => tile.f * ((PointF)GraphicsManager.CharSize).vecf();
        public static Entity At(this List<Entity> entities, int tile_x, int tile_y) => entities.FirstOrDefault(e => e.Position.ToTile() == (tile_x, tile_y).V());
        public static Entity At(this List<Entity> entities, vec tile) => entities.FirstOrDefault(e => e.Position.ToTile() == tile);
    }
}
