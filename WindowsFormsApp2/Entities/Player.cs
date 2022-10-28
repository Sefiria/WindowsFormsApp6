using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp2.Interfaces;
using WindowsFormsApp2.Properties;
using static WindowsFormsApp2.Entities.Enumerations;

namespace WindowsFormsApp2.Entities
{
    public class Player : DrawableEntity, IRP
    {
        #region Variables
        public int HPMax { get; set; } = 10;
        public int HP { get; set; } = 10;
        readonly Dictionary<Items, Bitmap> ItemsIco = new Dictionary<Items, Bitmap>()
        {
            [Items.Coin] = new Bitmap(Resources.coin, 24, 24),
            [Items.PlantAquarus] = new Bitmap(Resources.plant_aquarus, 24, 24),
            [Items.PlantSelanium] = new Bitmap(Resources.plant_selanium, 24, 24),
        };
        public const float Speed = 2F;
        public List<Marchandise> Stack { get; set; } = new List<Marchandise>();
        bool Minimap = false;
        public Dictionary<Key, bool> KeysUp { get; set; } = new Dictionary<Key, bool>()
        {
            [Key.M] = true,
            [Key.Left] = true,
            [Key.Right] = true,
            [Key.Up] = true,
            [Key.Down] = true,
            [Key.Space] = true,
            [Key.LeftCtrl] = true,
        };
        public Dictionary<Item, int> Inventory { get; set; } = new Dictionary<Item, int>();
        public Item[] Slots = new Item[8];
        public Dictionary<int, Rectangle> InventoryItemsList = new Dictionary<int, Rectangle>();
        public Dictionary<int, Rectangle> SlotsRect = new Dictionary<int, Rectangle>();
        public Stack<Items> AddItemsAnimStack { get; set; } = new Stack<Items>();
        readonly Dictionary<Key, (int SlotId, bool Released)> keySlots = new Dictionary<Key, (int SlotId, bool Released)>()
        {
            [Key.Y] = (0, true),
            [Key.U] = (1, true),
            [Key.I] = (2, true),
            [Key.O] = (3, true),
            [Key.H] = (4, true),
            [Key.J] = (5, true),
            [Key.K] = (6, true),
            [Key.L] = (7, true),
        };
        #endregion

        #region Ctor
        public Player(int TX = 0, int TY = 0)
        {
            Image = Resources.mainchar;
            Image.MakeTransparent();
            ItemsIco.Values.ToList().ForEach(i => i.MakeTransparent());

            X = TX * SharedCore.TileSize + SharedCore.TileSize / 2F;
            Y = TY* SharedCore.TileSize + SharedCore.TileSize / 2F;

            #region set slots rect
            var ts = (int)(SharedCore.TileSize * 1.5);
            int slotCount = Slots.Length;
            int xofst = 5;
            var x = SharedCore.RenderW / 2 - (slotCount * ts + xofst) / 2;
            var y = (int)(SharedCore.RenderH - ts * 1.5);
            for (int i = 0; i < slotCount; i++)
            {
                SlotsRect[i] = new Rectangle(x, y, ts, ts);
                x += ts + xofst;
            }
            #endregion
        }
        #endregion

        #region Draw
        public override void Draw()
        {
            SharedCore.g.DrawImage(Image, X - W / 2, Y - H / 2);
            //SharedCore.g.DrawRectangle(Pens.White, Tools.SnapToGrid(X), Tools.SnapToGrid(Y), SharedCore.TileSize, SharedCore.TileSize);

            DrawStack();

            if (Minimap)
                SharedData.World.DrawMinimap();

            DrawHpAndCoinsAndPlants();
            DrawInventory();
        }
        public void DrawStack()
        {
            for(int i=0; i<Stack.Count; i++)
            {
                SharedCore.g.DrawImage(Stack[i].Images[0], X - W / 2 + W / 2, Y - H / 2 - H / 2 - i * Stack[i].Images[0].Height / 2);
            }

            // draw add item stack anim
            DrawStackItem();
        }
        private void DrawHpAndCoinsAndPlants()
        {
            var g = SharedCore.g;
            int x = 10, y = 5, count;

            string hp = $"{HP} / {HPMax} HP";
            g.DrawString(hp, SharedCore.Font, Brushes.Gray, x, y);
            y += (int)g.MeasureString(hp, SharedCore.Font).Height + 10;

            foreach (var ico in ItemsIco)
            {
                var item = Inventory.FirstOrDefault(i => i.Key.ItemType == ico.Key);
                count = item.Key == null ? 0 : item.Value;
                g.DrawImage(ico.Value, x, y);
                x += ico.Value.Width + 5;
                g.DrawString(count.ToString(),
                             SharedCore.Font,
                             Brushes.Gray,
                             x,
                             y);
                x += (int)g.MeasureString(count.ToString(), SharedCore.Font).Width + 10;
            }
        }
        private void DrawInventory()
        {
            DrawSlots();

            if (!ShowInventory)
                return;

            var g = SharedCore.g;
            var font = SharedCore.Font;
            var boxX = SharedCore.RenderW * 0.1F;
            var boxY = SharedCore.RenderH * 0.1F;
            var boxW = SharedCore.RenderW * 0.8F;
            var boxH = SharedCore.RenderH * 0.6F;
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 50, 50, 50)), boxX, boxY, boxW, boxH);
            g.DrawRectangle(new Pen(Color.FromArgb(150, 80, 80, 80), 5F), boxX, boxY, boxW, boxH);

            int ofst = 5;
            int longestItemName = 0, currentItemNameW = 0, currentItemNameH = 0;
            int x = (int)boxX + ofst;
            int y = (int)boxY + ofst;
            int ite = 0;
            int countPerColumn = (int)boxH / (Inventory.Count + ofst);
            string name;
            int icoW = SharedCore.TileSize / 2 + ofst;
            foreach (var i in Inventory)
            {
                name = $"{Enum.GetName(typeof(Items), i.Key.ItemType)}{(i.Key is IDamager ? $" ({Enum.GetName(typeof(MaterialQuality), (i.Key as IDamager).Material)})" : "")} {i.Value}";
                y += ofst;
                g.DrawImage(new Bitmap(i.Key.Image, SharedCore.TileSize / 2, SharedCore.TileSize / 2), x, y);
                x += icoW;
                y -= ofst;
                g.DrawString(name, font, Brushes.White, x, y);
                x -= icoW;
                currentItemNameW = icoW + (int)g.MeasureString(name, font).Width;
                currentItemNameH = icoW + (int)g.MeasureString(name, font).Height / 4;
                if(InventoryItemsList.Count < Inventory.Count)
                    InventoryItemsList[ite] = new Rectangle(x, y + ofst, currentItemNameW, SharedCore.TileSize / 2);
                if (currentItemNameW > longestItemName)
                    longestItemName = currentItemNameW;
                ite++;
                if (ite >= countPerColumn)
                {
                    ite -= countPerColumn;
                    x = (int)boxX + ofst + longestItemName + ofst;
                    y = (int)boxY + ofst;
                }
                else
                {
                    y += currentItemNameH + ofst;
                }
            }
        }
        private void DrawSlots()
        {
            var g = SharedCore.g;
            Rectangle r;
            string c;
            int w;
            foreach (var rect in SlotsRect)
            {
                r = rect.Value;
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, 50, 50, 50)), r.X, r.Y, r.Width, r.Height);
                g.DrawRectangle(new Pen(Color.FromArgb(150, 80, 80, 80), 5F), r.X, r.Y, r.Width, r.Height);
                if (Slots[rect.Key] != null)
                {
                    g.DrawImage(new Bitmap(Slots[rect.Key].Image, r.Width - 10, r.Height - 10), r.X + 7, r.Y + 7);
                    c = Inventory.First(x => x.Key == Slots[rect.Key]).Value.ToString();
                    w = (int)g.MeasureString(c, SharedCore.Font).Width;
                    g.DrawString(c,
                                 SharedCore.Font,
                                 Brushes.Gray,
                                 r.X + r.Width / 2 - w / 2,
                                 r.Y + r.Height);
                }
            }
        }

        int _drawStackItem_step = 0;
        readonly int _drawStackItem_stepMax = 25;
        bool ShowInventory = false;
        private void DrawStackItem()
        {
            if (AddItemsAnimStack.Count == 0)
                return;

            Items item = AddItemsAnimStack.ElementAt(0);
            Bitmap img = new Bitmap(item.GetImageFromType(), SharedCore.TileSize / 4, SharedCore.TileSize / 4);
            SharedCore.g.DrawImage(img, X + W / 2, Y - 5 - _drawStackItem_step / (float)_drawStackItem_stepMax * SharedCore.TileSize);
            if (_drawStackItem_step >= _drawStackItem_stepMax)
            {
                _drawStackItem_step = 0;
                AddItemsAnimStack.Pop();
            }
            else
                _drawStackItem_step++;
        }
        #endregion

        #region Update
        public override void Update()
        {
            Inputs();
        }
        public void MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
        }
        private void Inputs()
        {
            if (Keyboard.IsKeyDown(Key.S) && !Tools.PositionTileIsOccupied(X, Y + H / 2 + Speed))
            {
                Y += Speed;
                Look.X = 0F;
                Look.Y = 1F;
            }
            if (Keyboard.IsKeyDown(Key.Z) && !Tools.PositionTileIsOccupied(X, Y - H / 2 - Speed))
            {
                Y -= Speed;
                Look.X = 0F;
                Look.Y = -1F;
            }
            if (Keyboard.IsKeyDown(Key.D) && !Tools.PositionTileIsOccupied(X + H / 2 + Speed, Y))
            {
                X += Speed;
                Look.X = 1F;
                Look.Y = 0F;
            }
            if (Keyboard.IsKeyDown(Key.Q) && !Tools.PositionTileIsOccupied(X - H / 2 - Speed, Y))
            {
                X -= Speed;
                Look.X = -1F;
                Look.Y = 0F;
            }

            if (Keyboard.IsKeyDown(Key.M) && KeysUp[Key.M])
            {
                Minimap = !Minimap;
                KeysUp[Key.M] = false;
            }

            if (Keyboard.IsKeyDown(Key.Space) && KeysUp[Key.Space])
            {
                (SharedData.Entities.Where(x => x is IActionable).FirstOrDefault(x =>
                (x.TX == TX && x.TY == TY - 1) ||
                (x.TX == TX && x.TY == TY + 1) ||
                (x.TY == TY - 1 && x.TY == TY) ||
                (x.TY == TY + 1 && x.TY == TY)
                ) as IActionable)?.Action();
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && KeysUp[Key.LeftCtrl])
            {
                ShowInventory = !ShowInventory;
            }

            UseSlots();
            InventorySetSlot();

#if DEBUG
            if (Keyboard.IsKeyDown(Key.Left) && KeysUp[Key.Left])
                SharedData.World.GoLeft();
            if (Keyboard.IsKeyDown(Key.Right) && KeysUp[Key.Right])
                SharedData.World.GoRight();
            if (Keyboard.IsKeyDown(Key.Up) && KeysUp[Key.Up])
                SharedData.World.GoTop();
            if (Keyboard.IsKeyDown(Key.Down) && KeysUp[Key.Down])
                SharedData.World.GoBottom();
#endif

            var dict = new Dictionary<Key, bool>(KeysUp);
            foreach (var kv in dict)
                KeysUp[kv.Key] = !Keyboard.IsKeyDown(kv.Key);
        }
        private void UseSlots()
        {
            if (ShowInventory)
                return;

            for (int i=0; i< keySlots.Count; i++)
            {
                var ks = keySlots.ElementAt(i);
                if (Keyboard.IsKeyDown(ks.Key))
                {
                    if (ks.Value.Released)
                    {
                        keySlots[keySlots.ElementAt(i).Key] = (ks.Value.SlotId, false);
                        if (Slots[ks.Value.SlotId] != null && Inventory.ContainsKey(Slots[ks.Value.SlotId]))
                        {
                            var item = Slots[ks.Value.SlotId];
                            if (!(item is IDrinkable) && !(item is IThrowable))
                                break;
                            (item as IDrinkable)?.Drink(this);
                            (item as IThrowable)?.Throw(this);
                            Inventory[Slots[ks.Value.SlotId]]--;
                            if (Inventory[item] == 0)
                            {
                                RemoveItemFromInventory(Slots[ks.Value.SlotId]);
                            }
                        }
                    }
                    break;
                }
                else
                {
                    keySlots[keySlots.ElementAt(i).Key] = (ks.Value.SlotId, true);
                }
            }
        }

        private void RemoveItemFromInventory(Item item)
        {
            foreach (var slot in Slots)
                if (slot != null && slot == item)
                    Slots[Slots.ToList().IndexOf(slot)] = null;
            Inventory.Remove(item);
        }

        private void InventorySetSlot()
        {
            if (ShowInventory)
            {
                foreach (var ks in keySlots)
                    if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(ks.Key)) { Slots[ks.Value.SlotId] = null; break; }

                foreach (var rect in InventoryItemsList)
                {
                    if (rect.Value.Contains(SharedCore.MouseLocation))
                    {
                        if (rect.Key >= Inventory.Count) continue;
                        var kv = Inventory.ElementAt(rect.Key);
                        if (kv.Key == null) continue;
                        var item = kv.Key;
                        foreach (var ks in keySlots)
                            if (Keyboard.IsKeyDown(ks.Key)) { Slots[ks.Value.SlotId] = item; break; }
                        break;
                    }
                }
            }
        }

        public void Hit(IDamager damager)
        {
            HP -= damager.Damage;
            if (HP <= 0)
            {
                HP = 0;
                Exists = false;
            }
        }
        #endregion

        #region Miscs Methods
        public void AddItems(Dictionary<Item, int> items, Logger.LogInventoryAddFrom from, bool doAnim = true)
        {
            AddItemsAnimStack.PushRange(items.Keys.Select(x => x.ItemType));

            Inventory.AddRange(items);

            Logger.LogInventoryAdd(items, from);
        }
        #endregion
    }
}
