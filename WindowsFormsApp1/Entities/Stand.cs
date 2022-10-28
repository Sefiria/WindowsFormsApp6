using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Properties;
using static WindowsFormsApp1.Entities.Enumerations;

namespace WindowsFormsApp1.Entities
{
    public class Stand : DrawableEntity
    {
        public string Nom;
        public Marchandise Marchandise;
        public List<Slot> Slots;
        public Stand(string Nom, Marchandise Marchandise, List<Slot> Slots, int TX = 0, int TY = 0) : base(TX * SharedCore.TileSize, TY * SharedCore.TileSize)
        {
            this.Nom = Nom;
            this.Marchandise = Marchandise;
            this.Slots = Slots;
        }

        public override void Draw()
        {
            var x = Tools.SnapToGrid((int)X);
            var y = Tools.SnapToGrid((int)Y);

            /*switch (Marchandise.Structure)
            {
                case MarchandiseStructure._1x1: SharedCore.g.DrawImage(Marchandise.Images[0], x, y); break;
                case MarchandiseStructure._2x1:
                    SharedCore.g.DrawImage(Marchandise.Images[0], x, y);
                    SharedCore.g.DrawImage(Marchandise.Images[1], x + SharedCore.TileSize, y);
                    break;
                case MarchandiseStructure._1x2:
                    SharedCore.g.DrawImage(Marchandise.Images[0], x, y);
                    SharedCore.g.DrawImage(Marchandise.Images[1], x, y + SharedCore.TileSize);
                    break;
                case MarchandiseStructure._2x2:
                    SharedCore.g.DrawImage(Marchandise.Images[0], x, y);
                    SharedCore.g.DrawImage(Marchandise.Images[1], x + SharedCore.TileSize, y);
                    SharedCore.g.DrawImage(Marchandise.Images[2], x, y + SharedCore.TileSize);
                    SharedCore.g.DrawImage(Marchandise.Images[3], x + SharedCore.TileSize, y + SharedCore.TileSize);
                    break;
            }*/

            SharedCore.g.DrawImage(Resources.standleft, x, y);
            SharedCore.g.DrawImage(Resources.standright, x + SharedCore.TileSize, y);
            var pi = new Bitmap(Marchandise.Images[0], 10, 10);// pi : product image
            SharedCore.g.DrawImage(pi, x + SharedCore.TileSize - pi.Width / 2, y);

            foreach (Slot slot in Slots)
                slot.Draw();
        }

        public override void Update()
        {
        }

        public List<Tile> GetTiles()
        {
            var result = new List<Tile>();
            //switch(Marchandise.Structure)
            //{
            //    case MarchandiseStructure._1x1: result.Add(Tools.GetTile(this)); break;
            //    case MarchandiseStructure._2x1: result.Add(Tools.GetTile(this));
            //                                    result.Add(new Tile(Tools.CoordToTileCoord(X) + 1, Tools.CoordToTileCoord(Y), this));
            //        break;
            //    case MarchandiseStructure._1x2: result.Add(Tools.GetTile(this));
            //                                    result.Add(new Tile(Tools.CoordToTileCoord(X), Tools.CoordToTileCoord(Y) + 1, this));
            //        break;
            //    case MarchandiseStructure._2x2: result.Add(Tools.GetTile(this));
            //                                    result.Add(new Tile(Tools.CoordToTileCoord(X) + 1, Tools.CoordToTileCoord(Y), this));
            //                                    result.Add(new Tile(Tools.CoordToTileCoord(X), Tools.CoordToTileCoord(Y) + 1, this));
            //                                    result.Add(new Tile(Tools.CoordToTileCoord(X) + 1, Tools.CoordToTileCoord(Y) + 1, this));
            //        break;
            //}
            result.Add(Tools.GetTile(this));
            result.Add(new Tile(Tools.CoordToTileCoord(X) + 1, Tools.CoordToTileCoord(Y), this));
            return result;
        }

        public bool AddProduct(Marchandise product)
        {
            var slot = GetAvailableSlot();
            if (slot != null)
                slot.Marchandise = product;
            return slot != null;
        }
        public Slot GetAvailableSlot() => Slots.FirstOrDefault(x => !x.IsFilled);
    }
}
