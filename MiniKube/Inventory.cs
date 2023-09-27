using MiniKube.Items;
using MiniKube.Structures;
using MiniKube.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniKube
{
    public class Inventory
    {
        public class Slot
        {
            public static Bitmap tex;
            public int x = 0, y = 0;
            public int w => Core.Cube;
            public int h => Core.Cube;
            /// <summary>
            /// Inventory Item ID
            /// </summary>
            public int ItemId = -1;
            public Bitmap ItemTex = null;
            public int Count = 0;
            public bool HasItem => ItemId > -1;
            public Slot()
            {
            }

            internal void DrawRect(Graphics g)
            {
                g.DrawImage(tex, x, y);
            }
            internal void DrawItem(Graphics g)
            {
                if (Count <= 0) { Count = 0; ItemId = -1; Core.Inventory.SortContentInSlots(); return; }
                if (HasItem)
                {
                    Bitmap tex = ItemTex;
                    var cb = Core.Cube;
                    if (tex.Width > cb || tex.Height > cb) tex = ItemTex = new Bitmap(ItemTex, new Size(Math.Min(tex.Width, cb), Math.Min(tex.Height, cb)));
                    int w = (Slot.tex.Width - tex.Width) / 2;
                    int h = (Slot.tex.Height - tex.Height) / 2;
                    if (tex != null) g.DrawImage(tex, x + 2 + w, y - 1 + h);
                    int fw = (int)g.MeasureString(Count.ToString(), Core.SmallFont).Width;
                    g.DrawString(Count.ToString(), Core.SmallFont, Brushes.White, x + Slot.tex.Width - fw, y - 10);
                }
            }
        }

        public Dictionary<int, int> Content = new Dictionary<int, int>();
        public List<Slot> Slots = new List<Slot>();
        public List<Slot> SlotsCatalog = new List<Slot>();
        public List<Slot> Slots_Available => Slots.Where(x => !x.HasItem).ToList();
        public List<Slot> Slots_Occupied => Slots.Where(x => x.HasItem).ToList();
        public bool HasAvailableSlot => Slots_Available.Count > 0;
        public Slot NextAvailableSlot => Slots_Available.FirstOrDefault();
        public List<Slot> SlotsCatalog_Available => SlotsCatalog.Where(x => !x.HasItem).ToList();
        public List<Slot> SlotsCatalog_Occupied => SlotsCatalog.Where(x => x.HasItem).ToList();
        public bool HasAvailableSlotCatalog => SlotsCatalog_Available.Count > 0;
        public Slot NextAvailableSlotsCatalog => SlotsCatalog_Available.FirstOrDefault();

        private Rectangle rect_content, rect_catalog;

        public Item GetByIdOfDefault(int id)
        {
            if (CheckContentBounds(id))
            {
                var type = Item.GetItemTypeById(id);
                if(type != null) return Activator.CreateInstance(type.BaseType == typeof(StructureBase) ? typeof(StructureBase) : type) as Item;
            }
            return null;
        }
        public bool IsOpen = false;

        Bitmap img_window = null;
        bool CheckContentBounds(int id) => Content.ContainsKey(id);

        public Inventory()
        {
            int cube = Core.Cube;
            int ofs = 6;
            int cube_wofs = cube + ofs;
            int w = (Core.iRW / 3 * 2) / (cube_wofs * 2) * (cube_wofs * 2) + ofs;
            int h = (Core.iRH / 3 * 2) / (cube_wofs * 2) * (cube_wofs * 2) + ofs;
            int x = Core.iRW / 3 / 2;
            int y = Core.iRH / 3 / 2;
            rect_content = new Rectangle(x, y - 3, w, h / 2);
            rect_catalog = new Rectangle(x, y + h / 2 + 3, w, h / 2);
            int cube_count_w = rect_content.Width / cube_wofs;
            int cube_count_h = rect_content.Height / cube_wofs;

            for (int j = 0; j < cube_count_h; j++)
            {
                for (int i = 0; i < cube_count_w; i++)
                {
                    Slots.Add(new Slot() { x = rect_content.X + ofs + i * cube_wofs, y = rect_content.Y + ofs + j * cube_wofs });
                    SlotsCatalog.Add(new Slot() { x = rect_catalog.X + ofs + i * cube_wofs, y = rect_catalog.Y + ofs + j * cube_wofs });
                }
            }

            Slot.tex = new Bitmap(cube - 1, cube - 1);
            using (Graphics gcube = Graphics.FromImage(Slot.tex))
            {
                int sz = 4, normal_gap = 5, gap = 5;
                SolidBrush brush = new SolidBrush(Color.FromArgb(75, 58, 37).ChangeSaturation(2));
                Color normal = Color.FromArgb(Math.Max(0, brush.Color.R - normal_gap), Math.Max(0, brush.Color.G - normal_gap), Math.Max(0, brush.Color.B - normal_gap));
                SolidBrush dark = new SolidBrush(Color.FromArgb(Math.Max(0, normal.R - gap), Math.Max(0, normal.G - gap), Math.Max(0, normal.B - gap)));
                SolidBrush bright = new SolidBrush(Color.FromArgb(normal.R + gap * 2, normal.G + gap * 2, normal.B + gap * 2));
                Pen pdark = new Pen(dark.Color);

                gcube.Clear(normal);
                gcube.FillRectangle(bright, 0, cube - sz, cube, cube);
                gcube.FillRectangle(bright, 0, 0, sz, cube);
                gcube.DrawLine(pdark, sz, cube - sz, cube - sz, cube - sz);
                gcube.DrawLine(pdark, sz, sz, sz, cube - sz);
                gcube.FillRectangle(dark, sz, 0, cube - sz / 2, sz);
                gcube.FillRectangle(dark, cube - sz, sz / 2, cube, cube - sz * 1.5F);
            }


            if (img_window == null)
            {
                img_window = new Bitmap(Core.iRW, Core.iRH);
                using (Graphics g = Graphics.FromImage(img_window))
                {
                    // window

                    SolidBrush brush = new SolidBrush(Color.FromArgb(75, 58, 37).ChangeSaturation(2));

                    g.FillRectangle(brush, rect_content);
                    g.DrawRectangle(new Pen(Color.Black, 5F), rect_content);
                    g.FillRectangle(brush, rect_catalog);
                    g.DrawRectangle(new Pen(Color.Black, 5F), rect_catalog);

                    // content / catalog

                    Slots.ForEach(slot => slot.DrawRect(g));
                    SlotsCatalog.ForEach(slot => slot.DrawRect(g));
                }
            }
        }
        public void Toggle()
        {
            IsOpen = !IsOpen;
        }
        public void Draw()
        {
            Core.g.DrawImage(img_window, 0, 0);
            SortContentInSlots();
            Slots.ForEach(slot => slot.DrawItem(Core.g));
            SlotsCatalog.ForEach(slot => slot.DrawItem(Core.g));
        }
        public bool AddItem<T>(int count) where T : Item => AddItem(Item.GetIdByItemType<T>(), count);
        public bool AddItem(int ID, int count)
        {
            SortContentInSlots();
            if (!HasAvailableSlot)
                return false;
            Content[ID] = (Content.ContainsKey(ID) ? Content[ID] : 0) + count;
            return true;
        }

        private void SortContentInSlots()
        {
            Slots.ForEach(x => x.ItemId = -1);
            Dictionary<int, int> ToDispatch = new Dictionary<int, int>(Content);
            int count;
            foreach (var kv in ToDispatch)
            {
                int key = kv.Key;
                int value = kv.Value;
                while (HasAvailableSlot && value > 0)
                {
                    if (!HasAvailableSlot)
                        break;
                    count = Core.StackSize;
                    var slot = Slots_Available.First();
                    slot.ItemId = key;
                    slot.ItemTex = Item.GetTexById(key);
                    slot.Count = count;
                    value -= count;
                }
            }
        }
    }
}
