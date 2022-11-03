using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Ores;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Timer Timer = new Timer() { Enabled = true, Interval = 10 };
        [JsonIgnore] Bitmap Image, CoinIco;
        List<IUI> UI = new List<IUI>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            CoinIco = Resources.coin;
            CoinIco.MakeTransparent();
            Core.g = Graphics.FromImage(Image);
            Core.W = Render.Width;
            Core.H = Render.Height;

            AddUI();

            Timer.Tick += Timer_Tick;
        }

        private void AddUI()
        {
            int x = 0;
            var serializer = new JsonSerializer();
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Objects
            };


            UI.Add(new UIButton(Resources.returnback.Transparent(), 10, 10));
            UI.Last().OnClick += (_s, _e) => { if (Data.Instance.State.PreviousState != null) Data.Instance.State = Data.Instance.State.PreviousState; };


            UI.Add(new UIButton("Inventaire", 0, 10));
            x = Core.W - UI.Last().W - 10;
            UI.Last().X = x;
            UI.Last().OnClick += (_s, _e) =>
            {
                Data.Instance.Inventory.Refresh();
                Data.Instance.Inventory.PreviousState = Data.Instance.World;
                Data.Instance.State = Data.Instance.Inventory;
            };

            UI.Add(new UIButton("Save", 0, 10));
            x -= UI.Last().W + 10;
            UI.Last().X = x;
            UI.Last().OnClick += (_s, _e) =>
            {
                var result = JsonConvert.SerializeObject(Data.Instance, settings);
                File.WriteAllText("data", result);
            };

            UI.Add(new UIButton("Load", 0, 10));
            x -= UI.Last().W + 10;
            UI.Last().X = x;
            UI.Last().OnClick += (_s, _e) =>
            {
                if (!File.Exists("data"))
                    return;
                Data.Instance = JsonConvert.DeserializeObject<Data>(File.ReadAllText("data"), settings);
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Update();
            Draw();
        }

        private new void Update()
        {
            Data.Instance.State.Update();

            if (Keyboard.IsKeyDown(Key.A))
                Data.Instance.State = Data.Instance.World;
            else if (Keyboard.IsKeyDown(Key.Z))
                Data.Instance.State = Data.Instance.Mine;
        }

        private void Draw()
        {
            Core.g.Clear(Color.Black);

            Data.Instance.State.Draw();

            Core.g.DrawImage(CoinIco, 60, 16);
            Core.g.DrawString(Data.Instance.StatInfo.Money.ToString(), DefaultFont, Brushes.White, 78, 18);
            int w = (int)Core.g.MeasureString(Data.Instance.StatInfo.Money.ToString(), DefaultFont).Width;
            DrawOres(88 + w);
            DrawUI();

            Render.Image = Image;
        }
        private void DrawOres(int x)
        {
            foreach(var ore in Data.Instance.StatInfo.OresCount)
            {
                Core.g.DrawImage(Ore.GetOreRes(ore.Key).Resized(16), x, 18);
                Core.g.DrawString(ore.Value.ToString(), DefaultFont, Brushes.White, x + 18, 18);
                x += (int)Core.g.MeasureString(ore.Value.ToString(), DefaultFont).Width + 28;
            }
        }
        private void DrawUI()
        {
            foreach (var ui in UI)
                ui.Draw();
        }

        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            Core.MousePosition = e.Location;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedUI = UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList();
            if (clickedUI.Count > 0)
            {
                clickedUI.ForEach(x => x.Clicked());
            }
            else
            {
                Data.Instance.State.MouseDown(e);
            }
        }
    }
}
