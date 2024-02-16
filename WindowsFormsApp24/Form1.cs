using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp24.Events;
using WindowsFormsApp24.Properties;
using WindowsFormsApp24.Scenes;
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
            MouseStates.Initialize();

            TempFirstInit();
        }

        private void NewFrame(object sender, EventArgs e)
        {
            KB.Update();

            Core.Instance.CurrentScene.Update();

            MouseStates.Update();

            if (WindowState != FormWindowState.Minimized)
                Core.Instance.ResetGraphics();
            Core.Instance.CurrentScene.Draw();
            if (WindowState != FormWindowState.Minimized)
                Core.Instance.WriteGraphics();
        }


        private void TempFirstInit()
        {
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;

            // Map
            scene.Maps.Add(new Map(16, 16, Resources.tileset));
            scene.CurrentMapID = 0;
            // Events
            scene.MainCharacter = new Character(Resources.mainchar, 5, 5, 0) { ShadowOffset = new PointF(-16, -6) };
            scene.Map.AddEvent(new Bag(NamedObjects.Carrot, 10000, 99, 2, 2, 0));
            scene.Map.AddEvent(new Shovel(1, 2, 0));
            var watercan = new WateringCan(3F, 0, 2, 0);
            watercan.Volume = watercan.Stats[WatercanStats.MaxVolume];
            scene.Map.AddEvent(watercan);
            scene.Map.AddEvent(new ClosetContainer(4, 3, 0));
            //scene.Map.AddEvent(new SmallContainer(4, 3, 0));
            //scene.Map.AddEvent(new MediumContainer(4, 5, 0));
            //scene.Map.AddEvent(new BigContainer(4, 7, 0));

            // DEBUG FILL CONTAINERS
            //for (int i = 0; i < SmallContainer.SmallContainerStackSizeX * SmallContainer.SmallContainerStackSizeY; i++)
            //{
            //    Core.MainCharacter.HandObject = scene.Map.AddEvent(new Seed(NamedObjects.Carrot, 5000, 0, 0));
            //    Map.GetEvent(typeof(SmallContainer)).PrimaryAction();
            //}
            //for (int i = 0; i < MediumContainer.MediumContainerStackSizeX * MediumContainer.MediumContainerStackSizeY; i++)
            //{
            //    Core.MainCharacter.HandObject = scene.Map.AddEvent(new Seed(NamedObjects.Carrot, 5000, 0, 0));
            //    Map.GetEvent(typeof(MediumContainer)).PrimaryAction();
            //}
            //for (int i = 0; i < BigContainer.BigContainerStackSizeX * BigContainer.BigContainerStackSizeY; i++)
            //{
            //    Core.MainCharacter.HandObject = scene.Map.AddEvent(new Seed(NamedObjects.Carrot, 5000, 0, 0));
            //    Map.GetEvent(typeof(BigContainer)).PrimaryAction();
            //}
            //for (int i = 0; i < ClosetContainer.ClosetStackSizeX * ClosetContainer.ClosetStackSizeY; i++)
            //{
            //    Core.MainCharacter.HandObject = scene.Map.AddEvent(new Seed(NamedObjects.Carrot, 5000, 0, 0));
            //    Map.GetEvent(typeof(ClosetContainer)).PrimaryAction();
            //}
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = e.Location;
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = true;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = false;
        }
    }
}
