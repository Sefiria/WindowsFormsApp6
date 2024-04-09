using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class Tile
    {
        public static Tile GetFromWorldLocation(vec tile_coords) => Core.Instance.SceneAdventure.World.GetTile(tile_coords);

        public vec Index;
        public vecf Coords => Index.f * GraphicsManager.TileSize;
        public int Resistance;
        public int Value;
        public Tile(vec index, int value, int resistance = -1)
        {
            Index = index;
            Value = value;
            Resistance = resistance;
            if (Resistance == -1)
                ResetAutoResistance();
        }
        public void ResetAutoResistance()
        {
            switch (Value)
            {
                case (int)DB.Tex.Grass: Resistance = 10; break;
                case (int)DB.Tex.Dirt: Resistance = 20; break;
                case (int)DB.Tex.Rock: Resistance = 50; break;
                case (int)DB.Tex.DeepRock: Resistance = 100; break;
                case (int)DB.Tex.HardRock: Resistance = 500; break;
                case (int)DB.Tex.Obsidian: Resistance = 5000; break;
                case (int)DB.Tex.UnbreakableRock: Resistance = int.MaxValue; break;
            }
        }
        public void Update()
        {
        }
        public void Draw(Graphics g)
        {
            GraphicsManager.DrawTile(g, this, Coords.i);
        }
    }
}
