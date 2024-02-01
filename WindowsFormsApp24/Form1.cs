using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp24.Events;
using WindowsFormsApp24.Properties;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24
{
    public partial class Form1 : Form
    {
        Timer GlobalTimer = new Timer() { Enabled=true, Interval=10 };

        public Form1()
        {
            InitializeComponent();

            GlobalTimer.Tick += NewFrame;
            Core.Instance.Render = Render;

            KB.Init();

            TempFirstInit();
        }

        private void NewFrame(object sender, EventArgs e)
        {
            KB.Update();

            Core.Instance.CurrentScene.Update();

            MouseStates.Update();
            MouseStates.ButtonDown = MouseButtons.None;

            Core.Instance.ResetGraphics();
            Core.Instance.CurrentScene.Draw();
            Core.Instance.WriteGraphics();
        }


        private void TempFirstInit()
        {
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;

            // Map
            scene.Maps.Add(new Map(16, 16, Resources.tileset));
            scene.CurrentMapID = 0;
            // Events
            scene.MainCharacter = new Character(Resources.mainchar, 5, 5);
            scene.Map.Events.Add(new Bag(NamedObjects.Carrot, 500, 1, 2, 2));
            scene.Map.Events.Add(new Shovel(1, 2));
            scene.Map.Events.Add(new WateringCan(3F, 0, 2) { Volume=3F });
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.Position = e.Location;
        }
    }
}
