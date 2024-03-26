using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _Console.Core.Utils.Maths;

namespace Mapping.Maps
{
    public class Map1 : Map
    {
        public Map1(Size size)
        {
            this.size = size;

            entities.AddRange(new[]
            {
                new Player(new Vec(20, size.Height / 2))
            });
        }
    }
}
