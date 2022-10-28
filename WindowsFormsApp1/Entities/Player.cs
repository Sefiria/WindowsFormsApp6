using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp1.Plants;
using WindowsFormsApp1.Properties;
using static WindowsFormsApp1.Entities.Enumerations;

namespace WindowsFormsApp1.Entities
{
    public class Player : DrawableEntity
    {
        public const float Speed = 2F;
        public List<Marchandise> Stack = new List<Marchandise>();
        public int MaxStack = 3;

        public Player(int TX = 0, int TY = 0) : base(TX * SharedCore.TileSize, TY * SharedCore.TileSize)
        {
            Image = Resources.mainchar;
            X += SharedCore.TileSize / 2F;
            Y += SharedCore.TileSize / 2F;
            Image.MakeTransparent();
        }

        public override void Draw()
        {
            SharedCore.g.DrawImage(Image, X - W / 2, Y - H / 2);
            //SharedCore.g.DrawRectangle(Pens.White, Tools.SnapToGrid(X), Tools.SnapToGrid(Y), SharedCore.TileSize, SharedCore.TileSize);

            DrawStack();
        }
        public void DrawStack()
        {
            for(int i=0; i<Stack.Count; i++)
            {
                SharedCore.g.DrawImage(Stack[i].Images[0], X - W / 2 + W / 2, Y - H / 2 - H / 2 - i * Stack[i].Images[0].Height / 2);
            }
        }

        public override void Update()
        {
            Inputs();

            if (Stack.Count < MaxStack)
            {
                var adj_plants = Tools.GetNearPlants(X, Y);
                foreach (Plant plant in adj_plants)
                {
                    var products = plant.GetFruits();
                    foreach (Marchandise product in products)
                    {
                        Stack.Add(product);
                        if (Stack.Count >= MaxStack)
                            break;
                    }
                    if (Stack.Count >= MaxStack)
                        break;
                }
            }

            if (Stack.Count > 0)
            {
                var adj_stands = Tools.GetNearStands(X, Y);
                foreach (Stand stand in adj_stands)
                {
                    DropProducts(stand);
                    if (Stack.Count == 0)
                        break;
                }

                if(Tools.NearToTrash(X, Y))
                    Stack.Clear();
            }
        }

        public void MouseDown(System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void Inputs()
        {
            if (Keyboard.IsKeyDown(Key.S) && !Tools.PositionTileIsOccupied(X, Y + H / 2 + Speed))
                Y += Speed;
            if (Keyboard.IsKeyDown(Key.Z) && !Tools.PositionTileIsOccupied(X, Y - H / 2 - Speed))
                Y -= Speed;
            if (Keyboard.IsKeyDown(Key.D) && !Tools.PositionTileIsOccupied(X + H / 2 + Speed, Y))
                X += Speed;
            if (Keyboard.IsKeyDown(Key.Q) && !Tools.PositionTileIsOccupied(X - H / 2 - Speed, Y))
                X -= Speed;
        }

        private void DropProducts(Stand stand)
        {
            Marchandise product = null;
            do
            {
                product = Stack.FirstOrDefault(x => x.Nom == stand.Marchandise.Nom);

                if (product == null)
                    return;

                var slot = stand.GetAvailableSlot();

                if (slot == null)
                    return;

                slot.Marchandise = product;
                Stack.Remove(product);
            }
            while (true);
        }
    }
}
