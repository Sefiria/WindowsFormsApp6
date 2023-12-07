using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;
using WindowsFormsApp22.Entities;
using static WindowsFormsApp22.Core;

namespace WindowsFormsApp22
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };

        UIPanel UIPanel_playerui;
        UIBar UIBar_cooldown_shotA, UIBar_cooldown_shotB;


        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.Map.InitializeInstance();

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Core.Image);

            Core.Player = new Player();

            CreateUI();
        }

        private void CreateUI()
        {
            UIPanel_playerui = new UIPanel() { Name = "playerui", Position = vecf.Zero, Size = RenderBounds.Size.Vf() };
            UIBar_cooldown_shotA = new UIBar() { Name = "cooldown_shotA", Position = (10F, 5F).Vf(), Size = (iRW - 20F, 5F).Vf(), OutlineColor = Color.White, FillColor = Color.White };
            UIBar_cooldown_shotB = new UIBar() { Name = "cooldown_shotB", Position = (10F, 15F).Vf(), Size = (iRW - 20F, 5F).Vf(), OutlineColor = Color.White, FillColor = Color.Yellow };
            
            UIBar_cooldown_shotA.UpdateValue = (ui) => (ui as UIBar).RangeValue.Value = Core.Player.cooldownA / (float)Core.Player.cooldown_maxA;
            UIBar_cooldown_shotB.UpdateValue = (ui) => (ui as UIBar).RangeValue.Value = Core.Player.cooldownB / (float)Core.Player.cooldown_maxB;
            UIBar_cooldown_shotA.RangeValue.Value = Core.Player.cooldownA;
            UIBar_cooldown_shotB.RangeValue.Value = Core.Player.cooldownB;
            
            UIPanel_playerui.Content.Add(UIBar_cooldown_shotA);
            UIPanel_playerui.Content.Add(UIBar_cooldown_shotB);

            UIMgt.UI.Add(UIPanel_playerui);
        }

        private void Update(object _, EventArgs e)
        {
            MouseStates.Position = Render.PointToClient(MousePosition);

            MgControls();

            Core.Map.Update();

            Core.EvtMgr.Update();

            UIMgt.Update();

            KB.Update();
            MouseStates.ButtonDown = MouseButtons.None;
        }

        private void MgControls()
        {
            if (!Focused)
                return;

            Core.Player.Controls();
        }

        private void Draw(object _, EventArgs e)
        {
            Core.g.Clear(Color.Black);
            Core.Map.Draw(g);
            DrawUI(Core.g);
            //if (Core.Inventory.IsOpen)
            //    Core.Inventory.Draw();

            Render.Image = Core.Image;

            Core.Ticks++;
        }
        private void DrawUI(Graphics g)
        {
            UIMgt.Draw(g);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
        }
    }
}
