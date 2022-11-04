using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Entities;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateFactory : IState
    {
        public int TileSz => 0;
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public List<Particule> Particules { get; set; } = new List<Particule>();
        public IState PreviousState { get; set; } = null;
        [JsonIgnore] public List<IUI> UI { get; set; } = new List<IUI>();
        public static List<(string Name, Bitmap Image, BigInteger Price, Action Action)> ToBuy;
        private static void Cross10Price(int index)
        {
            ToBuy[index] = (ToBuy[index].Name, ToBuy[index].Image, ToBuy[index].Price * 10, ToBuy[index].Action);
        }
         
        public StateFactory()
        {
            ToBuy = new List<(string Name, Bitmap Image, BigInteger Price, Action Action)>()
            {
                ("Unit", Resources.unit.Transparent(), new BigInteger(500), new Action(() => { Data.Instance.World.Entities.Add(new Unit(9.ToWorld() + Data.Instance.World.GetUnits().Count * 8, 18.ToWorld())); Cross10Price(0); })),
            };
            UI.Clear();
        }
        public void AddCategoryUI()
        {
            UIButton b;
            int x = 20, y = 50;

            b = new UIButton("Unités", x, y);
            b.ID = "Channel";
            UI.Add(b);
            x = b.X + b.W + 20;
            b.OnClick += (s, a) => RefreshUnits();
        }
        private void RefreshUnits() { RemoveUI(); AddUnitsUI(); }
        public void RemoveUI()
        {
            var list = UI.Where(x => x.ID != "Channel").ToList();
            foreach (var ui in list)
                UI.Remove(ui);
        }
        private void AddUnitsUI()
        {
            UIButton b;
            UILabel l;
            int x = 20, y = 100;

            foreach (var item in ToBuy)
            {
                b = new UIButton(item.Image, x, y);
                b.ID = "Unités";
                UI.Add(b);
                x = b.X + b.W + 10;
                l = new UILabel(item.Price.ToString(), x, y);
                l.ID = "Unités";
                x = l.X + l.W + 25;
                UI.Add(l);
                if (x > Core.H * 0.8F)
                {
                    x = 20;
                    y += b.H + 20;
                }
                b.OnClick += (s, a) => { if (Data.Instance.StatInfo.Money >= item.Price) { Data.Instance.StatInfo.Money -= item.Price; item.Action(); RefreshUnits(); } };
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
                ui.Draw();
        }

        public void MouseDown(MouseEventArgs e)
        {
            UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList().ForEach(x => x.Clicked());
        }

        public void ReturnToPreviousState()
        {
            Data.Instance.State = PreviousState;
            PreviousState = null;
        }
    }
}
