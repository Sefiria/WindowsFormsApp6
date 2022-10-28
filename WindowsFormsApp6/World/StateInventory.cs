using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.UI;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateInventory : IState
    {
        public int TileSz => 0;
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public List<Particule> Particules { get; set; } = new List<Particule>();
        public List<IUI> UI { get; set; } = new List<IUI>();

        public StateInventory()
        {
        }
        public void Refresh()
        {
            UI.Clear();
            UIButton b;
            int x = 20, y = 100;
            var inv = Data.Instance.StatInfo.Inventory;

            foreach (var item in inv.Pickaxes)
            {
                b = new UIButton(item.ItemRef.Image.Resized(16), x, y);
                if(inv.Pickaxes.IndexOf(item) == inv.UsedPickaxeID)
                    b.ID = "Used";
                b.OnClick += (s, a) =>
                {
                    inv.UsedPickaxeID = inv.Pickaxes.IndexOf(item);
                    Refresh();
                };
                UI.Add(b);

                x = b.X + b.W + 10;

                if (x > Core.H * 0.8F)
                {
                    x = 20;
                    y += (b.H + 20) * 2;
                }
            }
        }

        public void Update()
        {
        }

        public void DrawStatic()
        {
        }

        public void Draw()
        {
            foreach (var ui in UI)
                if(ui.ID == "Used")
                    ui.Draw(null, Color.Yellow);
                else
                    ui.Draw(null);
        }

        public void MouseDown(MouseEventArgs e)
        {
            UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList().ForEach(x => x.Clicked());
        }
    }
}
