using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp11.Items;

namespace WindowsFormsApp11
{
    public class CraftingTable
    {
        public static List<(Rectangle Rect, Type Type)> ItemsRectType;
        public static Rectangle CraftBox;
        public static int DraggingId = -1;
        private static Point MousePosition;
        public static List<Item> Table;
        public static int MaxTableCount = 5;

        public static void Initialize()
        {
            ItemsRectType = new List<(Rectangle Rect, Type Type)>();

            int sz = 100;
            int hsz = sz / 2;
            CraftBox = new Rectangle(Var.W / 2 - hsz, Var.H / 2 - hsz, sz, sz);
        }
        public static void Open()
        {
            Table = new List<Item>();
        }
        public static void Close()
        {
            Var.Data.Ship.Mod(Table);
        }

        public static void MouseLeave(object sender, EventArgs e)
        {
            DraggingId = -1;
        }
        public static void MouseDown(object sender, MouseEventArgs e)
        {
            if (Table.Count < MaxTableCount * MaxTableCount)
            {
                DraggingId = ItemsRectType.IndexOf(ItemsRectType.FirstOrDefault(x => x.Rect.Contains(e.Location)));
                if (DraggingId != -1 && Var.Data.Ship.ItemPackage.CountOf(ItemsRectType[DraggingId].Type) == 0)
                    DraggingId = -1;
            }
        }
        public static void MouseUp(object sender, MouseEventArgs e)
        {
            if (DraggingId != -1 && CraftBox.Contains(e.Location))
            {
                Var.Data.Ship.ItemPackage.Remove(ItemsRectType[DraggingId].Type);
                AddItemToTable(ItemsRectType[DraggingId].Type);
            }

            DraggingId = -1;
        }
        public static void MouseMove(object sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
        }

        public static void Update()
        {
        }
        public static void Draw()
        {
            UICraftingTable.Draw();

            if (DraggingId != -1)
            {
                var c = GetColorFromItemType(ItemsRectType[DraggingId].Type);
                Var.g.FillRectangle(new SolidBrush(c), MousePosition.X, MousePosition.Y, 20, 20);
                Var.g.DrawRectangle(Pens.White, MousePosition.X, MousePosition.Y, 20, 20);
            }
        }

        private static Color GetColorFromItemType(Type type)
        {
            switch(Activator.CreateInstance(type))
            {
                default:
                case ItemBlue blue: return Color.Blue;
                case ItemGreen green: return Color.Lime;
                case ItemRed red: return Color.Red;
                case ItemYellow yellow: return Color.Yellow;
                case ItemBlack black: return Color.Black;
                case ItemWhite white: return Color.White;
            }
        }

        private static void AddItemToTable(Type type)
        {
            Table.Add((Item)Activator.CreateInstance(type));
        }
    }
}
