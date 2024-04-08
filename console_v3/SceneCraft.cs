using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static console_v3.TheRecipes;

namespace console_v3
{
    public class SceneCraft : Scene
    {
        public class ListItem
        {
            public static Font font => GraphicsManager.BigFont;
            public static Font MidFont => GraphicsManager.MidFont;
            public static Font MiniFont => GraphicsManager.MiniFont;
            public static int sz = 50;

            public int Index;
            public int x, y;
            public vec vec { get => (x, y).V(); set { x = value.x; y = value.y; } }
            public Guid content = Guid.Empty;
            public string Name;
            public string DisplayName => $"{Name} ( {Count} )";
            public int DBRef = -1, Count = 0;
            public virtual Rectangle Bounds => new Rectangle(20, (int)(20 + (Index - scroll) * (TextRenderer.MeasureText("A", font).Height * 1.5f)), TextRenderer.MeasureText("A", font).Width * 2 + TextRenderer.MeasureText(DisplayName, font).Width, TextRenderer.MeasureText("A", font).Height);
            public ListItem() { }
            public ListItem(int index)
            {
                Index = index;
            }
            public ListItem(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public ListItem(ListItem copy)
            {
                Index = copy.Index;
                x = copy.x;
                y = copy.y;
                vec = copy.vec;
                content = copy.content;
                Name = copy.Name;
                DBRef = copy.DBRef;
                Count = copy.Count;
            }
            public virtual void Draw(Graphics g, Graphics gui)
            {
                if (Count > 0)
                    DrawBoundsRelief(gui, Bounds, 4f, 4, 4, Bounds.Contains(MouseStates.Position.ToPoint().Minus(listrect.Location)) ? ((SelectedTempItem?.Index ?? -1) == Index ? brushLight : null) : brushDark);
                int i = Index - (int)scroll;
                if (i < 0f || i >= objs.Count) return;
                DrawItemContent(gui, Point.Empty);
            }
            public void DrawItemContent(Graphics g, Point offset)
            {
                var pos = offset != Point.Empty ? offset : Bounds.Location;
                var sz = TextRenderer.MeasureText("A", font);
                int w = sz.Width;
                int h = sz.Height;
                int y = (int)(20 + (Index - scroll) * (h * 1.5f));

                if(DBRef > -1)
                    g.DrawImage(DB.GetTexture(DBRef, 32), pos);
                if(offset != Point.Empty)
                    g.DrawString(DisplayName, font, Count > 0 ? Brushes.White : Brushes.Gray, w * 2 + offset.X, offset.Y);
                else
                    g.DrawString(DisplayName, font, Count > 0 ? Brushes.White : Brushes.Gray, 20 + w * 2, y);
            }

            public static void DrawBoundsRelief(Graphics g, Rectangle r, float w, int inflate = 0, int offset = 0, Brush brush = null)
            {
                if(inflate >0) r.Inflate(inflate, inflate);
                if(offset > 0) r.Offset(offset, offset);
                g.FillRectangle(brush ?? brushMid, r);
                Pen p = new Pen(colorDark.Mod(30, 25, 20), w - 1);
                g.DrawLine(p, r.Left, r.Top, r.Right - w, r.Top);
                g.DrawLine(p, r.Left, r.Top, r.Left, r.Bottom - w);
                p = new Pen(colorDark.Mod(0, -10, -15), w - 1);
                g.DrawLine(p, r.Right - w, r.Bottom - w, r.Left, r.Bottom - w);
                g.DrawLine(p, r.Right - w, r.Bottom - w, r.Right - w, r.Top);
            }
            public ListItem Clone() => new ListItem(this);
        }
        public class Slot : ListItem
        {
            private float opacity = 1f;
            public override Rectangle Bounds => new Rectangle((mainrect.Location.vecf().i + mainrect.Size.V() / 2 - CraftSize * (sz + slot_margin) / 2 + vec * (sz + slot_margin)).ipt, new Size(sz, sz));
            public Slot() : base()
            {
            }
            public Slot(int x, int y) : base(x, y)
            {
            }
            public override void Draw(Graphics g, Graphics gui)
            {
                var bounds = Bounds;

                if((SelectedTempItem?.Index ?? -1) > -1 && bounds.Contains(MouseStates.Position.ToPoint()))
                    DrawBoundsRelief(g, bounds, 4f, 2, brush:brushLight);
                else
                    DrawBoundsRelief(g, bounds, 4f, 2);

                if(DBRef > -1)
                    g.DrawImage(DB.GetTexture(DBRef, 28), bounds.Location);

                if (Count > 0)
                {
                    g.DrawString(Count.ToString(), MiniFont, Brushes.White, bounds.Location.Plus((Point)TextRenderer.MeasureText("A", font)));
                    if (Bounds.Contains(MouseStates.Position.ToPoint()))
                    {
                        DrawHint(gui);
                        if(opacity > 0.5f )
                            opacity -= 0.01f;
                    }
                    else
                        opacity = 1f;
                }
                else
                    opacity = 1f;
            }
            private void DrawHint(Graphics gui)
            {
                string name = listItems?.FirstOrDefault(it => it.content == content)?.Name;
                if (string.IsNullOrWhiteSpace(name))
                    name = Name;
                if (string.IsNullOrWhiteSpace(name))
                    return;
                var text = name + " x " + Count;
                if(string.IsNullOrWhiteSpace(text))
                    return;
                var font = MidFont;
                var sz = TextRenderer.MeasureText(text, font);
                var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
                var margin = 10;
                var rect = new Rectangle(position.Minus(margin, margin / 2), new Size(sz.Width + margin * 2, sz.Height + margin));
                byte _opacity = (byte)(opacity * byte.MaxValue).ByteCut();
                var brush = new SolidBrush(Color.FromArgb((byte)(opacity * 1.25f * byte.MaxValue).ByteCut(), Color.LightGray));
                gui.FillRectangle(new SolidBrush(Color.FromArgb(_opacity, colorLight)), rect);
                gui.DrawRectangle(new Pen(Color.FromArgb(_opacity, colorMid)), rect);
                gui.DrawString(text, font, brush, position);
            }
        }
        public class SlotResult : Slot
        {
            public override Rectangle Bounds => new Rectangle(mainrect.X + mainrect.Width / 2 - SlotsResult.Count * (sz + slot_margin) / 2 + x * (sz + slot_margin), mainrect.Y + mainrect.Height / 2 + CraftSize * (sz + slot_margin) / 2 + 50, sz, sz);
            public RecipeObj[,] Needs;
            public Recipe Recipe;
            public SlotResult(RecipeObj[,] needs, RecipeObj result) : base()
            {
                Needs = needs;
                DBRef = result.DBRef;
                Count = result.Count;
                Name = DB.DefineName(DBRef);
            }
        }

        public List<Slot> Slots = new List<Slot>();
        public static List<SlotResult> SlotsResult = new List<SlotResult>();

        int w;
        int h;
        public static int CraftSize;
        public static Rectangle mainrect;
        public static Rectangle listrect;
        public static int slot_margin = 2;
        public static Color colorDark = Color.FromArgb(40, 30, 20);
        public static Color colorMid = Color.FromArgb(60, 40, 25);
        public static Color colorLight = Color.FromArgb(80, 50, 30);
        public static SolidBrush brushDark, brushMid, brushLight;
        public static int hint_z_s;
        public static Point ItemListSelectedPoint;

        public static float scroll = 0f;
        private static List<Guid> objs;
        private static List<ListItem> listItems;
        private static ListItem SelectedTempItem = null;

        public SceneCraft(int size)
        {
            CraftSize = size;
            Initialize();
        }

        public override void Initialize()
        {
            var m = 50;
            w = Core.Instance.ScreenWidth - m * 2;
            h = Core.Instance.ScreenHeight - m * 2;
            listrect = new Rectangle(m, m, 300, h);
            mainrect = new Rectangle(listrect.Right, m, w - listrect.Width, h);
            brushDark = new SolidBrush(colorDark);
            brushMid = new SolidBrush(colorMid);
            brushLight = new SolidBrush(colorLight);
            hint_z_s = byte.MaxValue;

            new RecipeFactory(); // dummy instantiation for trigger static ctor

            ResetListAndSlots();
        }

        private void ResetListAndSlots(bool keepSlotsIfPossible = false)
        {
            var inv = Core.Instance.TheGuy.Inventory;
            var items = inv.Items.Select(it => it.UniqueId);
            var tools = inv.Tools.Select(t => t.UniqueId);
            objs = tools.Concat(items).ToList();
            List<Slot> prevSlots = null;

            if(keepSlotsIfPossible)
                prevSlots = new List<Slot>(Slots);

            Slots.Clear();
            for (int y = 0; y < CraftSize; y++)
            {
                for (int x = 0; x < CraftSize; x++)
                {
                    Slots.Add(new Slot(x, y));
                }
            }

            listItems = objs.Select(obj =>
            {
                (string name, int dbref, int count, Guid content) = inv.GetFullInfosByUniqueId(obj);
                return new ListItem(objs.IndexOf(obj)) { Name = name, DBRef = dbref, Count = count, content = content };
            }).ToList();

            if(keepSlotsIfPossible)
            {
                foreach (var prevSlot in prevSlots)
                {
                    if (prevSlot.content == Guid.Empty)
                        continue;
                    var item = listItems.FirstOrDefault(it => it.content == prevSlot.content);
                    if (item != null)
                    {
                        int id = Slots.IndexOf(Slots.First(slot => slot.x == prevSlot.x && slot.y == prevSlot.y));
                        int count = Math.Min(prevSlot.Count, item.Count);
                        if (count == 0)
                            continue;
                        Slots[id].Index = prevSlot.Index;
                        Slots[id].Name = prevSlot.Name;
                        Slots[id].DBRef = prevSlot.DBRef;
                        Slots[id].content = prevSlot.content;
                        Slots[id].Count = count;
                        listItems[listItems.IndexOf(item)].Count -= count;
                    }
                }
            }
        }

        public override void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Tab) || KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.Instance.SwitchScene(Core.Scenes.Adventure);
                return;
            }

            if (KB.IsKeyDown(KB.Key.Z)) scroll -= 0.2f;
            if (KB.IsKeyDown(KB.Key.S)) scroll += 0.2f;
            scroll = Math.Max(0, Math.Min(objs.Count - 1, scroll));

            var msbase = MouseStates.Position.ToPoint();
            var ms = msbase.Minus(listrect.Location);
            var bt_left = MouseStates.IsButtonPressed(MouseButtons.Left);
            var bt_right = MouseStates.IsButtonPressed(MouseButtons.Right);

            # region mainalgo
            if (bt_left || bt_right)// left / right click
            {
                if (bt_left && SelectedTempItem == null)// left click & no selection
                {
                    var item = listItems.FirstOrDefault(it => it.Bounds.Contains(ms));
                    if (item != null && item.Count > 0)// click on item in list
                    {
                        ItemListSelectedPoint = ms.Minus(item.Bounds.Location);
                        SelectedTempItem = item.Clone();
                        listItems[item.Index].Count = 0;
                    }
                    else if(item == null)// no item clicked
                    {
                        var slot = Slots.FirstOrDefault(s => s.Bounds.Contains(msbase));
                        if(slot != null && slot.content != Guid.Empty)// slot clicked (take full slot content)
                        {
                            ListItem inter = slot.Clone();
                            SelectedTempItem = new ListItem();
                            SelectedTempItem.Index = inter.Index;
                            SelectedTempItem.Name = inter.Name;
                            SelectedTempItem.content = inter.content;
                            SelectedTempItem.Count = inter.Count;
                            SelectedTempItem.DBRef = inter.DBRef;
                            slot.Index = -1;
                            slot.content = Guid.Empty;
                            slot.Count = 0;
                            slot.DBRef = -1;
                        }
                    }
                }
                else// no left click OR already selection
                {
                    if (listrect.Contains(ms) && SelectedTempItem != null)// list rectangle contains mouse & already selection
                    {
                        if (bt_left)// left click
                        {
                            listItems[SelectedTempItem.Index].Count += SelectedTempItem.Count;
                            SelectedTempItem = null;
                        }
                        else if (bt_right)// right click
                        {
                            listItems[SelectedTempItem.Index].Count++;
                            SelectedTempItem.Count--;
                            if(SelectedTempItem.Count == 0)
                                SelectedTempItem = null;
                        }
                    }
                    else if(mainrect.Contains(ms))// main rectangle contains mouse
                    {
                        var slot = Slots.FirstOrDefault(s => s.Bounds.Contains(msbase));
                        if (slot != null)// slot hover
                        {
                            void setslot(int count)
                            {
                                slot.Index = SelectedTempItem.Index;
                                slot.Name = SelectedTempItem.Name;
                                slot.content = SelectedTempItem.content;
                                slot.Count = count;
                                slot.DBRef = SelectedTempItem.DBRef;
                            }

                            if (SelectedTempItem != null && (slot.content == Guid.Empty || (SelectedTempItem?.Index ?? -1) == slot.Index))// already selection & ( slot same as selection OR slot empty )
                            {
                                if (bt_left)// left click
                                {
                                    if (slot.content == Guid.Empty)// slot empty
                                    {
                                        setslot(SelectedTempItem.Count);
                                        SelectedTempItem = null;
                                    }
                                    else// slot not empty
                                    {
                                        if (SelectedTempItem.Index == slot.Index)// slot same as selection
                                        {
                                            slot.Count += SelectedTempItem.Count;
                                            SelectedTempItem = null;
                                        }
                                    }
                                }
                                else if (bt_right)// slot different than selection & right click
                                {
                                    if (slot.content == Guid.Empty)// slot empty
                                        setslot(1);
                                    else// slot not empty
                                        slot.Count++;
                                    SelectedTempItem.Count--;
                                    if (SelectedTempItem.Count == 0)
                                        SelectedTempItem = null;
                                }
                            }
                            else if (bt_left)// left click
                            {
                                ListItem inter = slot.Clone();
                                setslot(SelectedTempItem.Count);
                                SelectedTempItem.Index = inter.Index;
                                SelectedTempItem.Name = inter.Name;
                                SelectedTempItem.content = inter.content;
                                SelectedTempItem.Count = inter.Count;
                                SelectedTempItem.DBRef = inter.DBRef;

                            }
                        }
                    }
                }
            }
            #endregion

            CheckRecipes();

            if(bt_left && SlotsResult.Any(slot => slot.Bounds.Contains(msbase)))
            {
                var inv = Core.Instance.TheGuy.Inventory;
                var needs = SlotsResult.First().Needs;
                foreach (var need in needs)
                {
                    if(need == null)
                        continue;
                    if (need.DBRef.IsTool())
                    {
                        // TODO : give damages to the tool (ATM tool usePoint not implemented)
                        continue;// don't remove tools but give them damage
                    }
                    for (int i = 0; i < need.Count; i++)
                        inv.RemoveOne(need.DBRef);
                }
                foreach (var result in SlotsResult)
                    inv.Add((result.DBRef, result.Count));
                ResetListAndSlots(true);
            }
        }

        private void CheckRecipes()
        {
            SlotsResult.Clear();
            var slots = new RecipeObj[CraftSize, CraftSize];
            Slot _Slot;
            for (int y = 0; y < CraftSize; y++)
            {
                for (int x = 0; x < CraftSize; x++)
                {
                    _Slot = Slots[y * CraftSize + x];
                    slots[x, y] = _Slot.content == Guid.Empty ? null : new RecipeObj(listItems.First(it => it.content == _Slot.content).DBRef, _Slot.Count);
                }
            }
            if (Recipes != null)
            {
                Recipe recipe = Recipes.FirstOrDefault(r => r.SatisfiedBy(slots));
                int x = 0;
                if (recipe != null)
                    foreach (var result in recipe.Results)
                        SlotsResult.Add(new SlotResult(recipe.Needs, result) { x = x++ });
            }
        }

        public override void Draw(Graphics g, Graphics gui)
        {
            g.FillRectangle(brushDark, listrect);
            g.DrawRectangle(Pens.Black, listrect);
            g.FillRectangle(brushDark, mainrect);
            g.DrawRectangle(Pens.Black, mainrect);

            Slots.ForEach(slot => slot.Draw(g, gui));

            Bitmap listBitmap = new Bitmap(listrect.Width, listrect.Height);
            Graphics listgui = Graphics.FromImage(listBitmap);
            var inv = Core.Instance.TheGuy.Inventory;
            Font font = new Font("Segoe UI", 14f);
            var sz = TextRenderer.MeasureText("A", font);
            int w = sz.Width;
            int h = sz.Height;

            listItems.ForEach(listitem => listitem.Draw(null, listgui));

            if (hint_z_s > 0)
            {
                listgui.FillRectangle(new SolidBrush(Color.FromArgb(hint_z_s, colorDark)), listrect.Width / 2 - GraphicsManager.TileSize / 2, 0, GraphicsManager.TileSize * 2, GraphicsManager.TileSize * 3);
                listgui.DrawString("↑", GraphicsManager.FontSQ, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - GraphicsManager.TileSize / 2 + 5, 0);
                listgui.DrawString("Z", GraphicsManager.FontSQ, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan))  , listrect.Width / 2 - 5, GraphicsManager.TileSize * 1.5f);
                listgui.FillRectangle(new SolidBrush(Color.FromArgb(hint_z_s, colorDark)), listrect.Width / 2 - GraphicsManager.TileSize / 2, listrect.Height - GraphicsManager.TileSize * 3, GraphicsManager.TileSize * 2, GraphicsManager.TileSize * 3);
                listgui.DrawString("S", GraphicsManager.FontSQ, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - GraphicsManager.TileSize / 2 + 5, listrect.Height - GraphicsManager.TileSize * 2);
                listgui.DrawString("↓", GraphicsManager.FontSQ, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - 5, listrect.Height - GraphicsManager.TileSize * 2 + GraphicsManager.TileSize);
                hint_z_s -= 3;
            }
            gui.DrawImage(listBitmap, listrect.Location);

            if (SelectedTempItem  != null)
            {
                var item = SelectedTempItem;
                var rect = item.Bounds;
                rect = new Rectangle(MouseStates.Position.ToPoint().Minus(ItemListSelectedPoint), rect.Size);
                ListItem.DrawBoundsRelief(gui, rect, 4f, 4, 4, brushLight);
                item.DrawItemContent(gui, rect.Location);
            }

            SlotsResult.ForEach(slot => slot.Draw(g, gui));
        }
    }
}
