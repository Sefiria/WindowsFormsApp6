﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Shovel : Event
    {
        internal Shovel(float x, float y) : base(Core.NamedTextures[NamedObjects.Shovel], true, x, y){}

        internal override void SecondaryAction()
        {
            PredefinedAction_TakeDrop(this);
        }
        internal override void PrimaryAction()
        {
            if (Core.CurrentMainScene.MainCharacter.HandObject != ID)
                return;
            int x = (int)((float)Data["X"]) / Core.TileSize;
            int y = (int)((float)Data["Y"]) / Core.TileSize;
            var tiledata = Map.Current.Tiles.PointedData(x, y);
            var index = tiledata.index;
            var layer = tiledata.layer;
            if (index == 0 && Map.Current.Events.FirstOrDefault(ev => ev.TileX != x && ev.TileY != y) != null)
                Map.Current.Tiles.Set(layer+1, x, y, 1);
        }
    }
}