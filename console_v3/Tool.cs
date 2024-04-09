using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v3
{
    public class Tool : ITool, IName, IDBItem, IUniqueRef
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unnamed_Tool";
        public int DBRef { get; set; }
        public int DBRef_Ore;
        public float Duration;
        public int Count;
        public int STR;
        public Bitmap Image;

        public Tool()
        {
        }
        public Tool(Tool copy)
        {
            Name = copy.Name;
            Duration = copy.Duration;
            DBRef = copy.DBRef;
            Count = copy.Count;
            Image = ResetGraphics(DBRef, DBRef_Ore);
        }
        public Tool(string name, int dbref, int dbref_ore, int AddedSTR = 0)
        {
            if(!dbref_ore.IsOre())
                dbref_ore =  (int)DB.Tex.Wood;

            Name = name ?? $"{DB.DefineName(dbref_ore)} { DB.DefineName(dbref)}";
            Duration = 1f;
            DBRef = dbref;
            DBRef_Ore = dbref_ore;
            STR = DB.GetOre(DBRef_Ore).ToolQuality + AddedSTR;
            Count = 1;
            Image = ResetGraphics(DBRef, DBRef_Ore);
        }
        public static Bitmap ResetGraphics(int dbref, int dbref_ore)
        {
            Bitmap Image = null;
            var ore = DB.GetOre(dbref_ore);
            if (ore != null)
            {
                Image = DB.GetTexture(dbref)
                                  .ChangeColor(Color.FromArgb(1, 0, 0), Color.FromArgb(ore.ColorDark))
                                  .ChangeColor(Color.FromArgb(0, 1, 0), Color.FromArgb(ore.ColorMid))
                                  .ChangeColor(Color.FromArgb(0, 0, 1), Color.FromArgb(ore.ColorLight));
            }
            return Image;
        }

        public void Use(Entity triggerer)
        {
            if(DBRef.IsShovel()) UseShovel(triggerer);
        }

        private void UseShovel(Entity triggerer)
        {
            void loot(int v)
            {
                // missing FibreDePlante & Boue in DB
                //int n;
                //switch(chunk.Tiles[tile].Value)
                //{
                //    case (int)DB.TexName.Grass:
                //        n = RandomThings.rnd(3);
                //        if (n > 0) triggerer.Inventory.Add(new Item("Fibre De Plante", (int)DB.TexName.FibreDePlante, n));
                //        break;
                //    case (int)DB.TexName.Dirt:
                //        n = RandomThings.rnd(2);
                //        if (n > 0) triggerer.Inventory.Add(new Item("Boue", (int)DB.TexName.Boue, n));
                //        break;
                //}
            }
            var tile = triggerer.Position.ToTile();
            var chunk = Core.Instance.SceneAdventure.World.GetChunk();
            if (chunk.Tiles[tile].Value > 0)
            {
                chunk.Tiles[tile].Resistance -= STR;
                if (chunk.Tiles[tile].Resistance <= 0)
                {
                    ParticlesManager.Generate(tile.ToWorld() + GraphicsManager.TileSize / 2f, 3f, 4f, Color.FromArgb(DB.PxColors[chunk.Tiles[tile].Value]), 5, 100);
                    loot(chunk.Tiles[tile].Value);
                    chunk.Tiles[tile].Value--;
                    chunk.Tiles[tile].ResetAutoResistance();
                }
                else
                {
                    ParticlesManager.Generate(tile.ToWorld() + GraphicsManager.TileSize / 2f, 1f, 2f, Color.FromArgb(DB.PxColors[chunk.Tiles[tile].Value]), 3, 100);
                }
            }
        }
    }
}
