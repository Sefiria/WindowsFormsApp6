using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp24
{
    internal class Tile : ITile
    {
        internal int layer, x, y;
        public  int TilesetIndex { get; set; }
        private float m_wet = 0F;
        public float wet
        {
            get => m_wet;
            set
            {
                m_wet = value;
                if (m_wet > 1F)
                    m_wet = 1F;
                Map.Current.TilesToRefresh.Enqueue((layer, x, y));
            }
        }
        internal float liquid_absorption = 0.9999F;// loose 1F - liquid_absorption of wet per chunked tick

        public Tile(int tilesetIndex, int layer, int x, int y)
        {
            TilesetIndex = tilesetIndex;
            this.layer = layer;
            this.x = x;
            this.y = y;
        }
    }
}
