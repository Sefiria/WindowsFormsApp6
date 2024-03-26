using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;
using Framework;
using Framework.Entities._Entity._Organic._Playable;
using zelecx;
using System.IO;

namespace Play
{
    public partial class MainForm : Form
    {
        static private MainForm m_form = null;
        static public MainForm GetPlayMainForm()
        {
            if (m_form == null)
                m_form = new MainForm();
            return m_form;
        }
        static public void KillInstance()
        {
            m_form = null;
        }

        public RenderWindow Render;
        public Level level;
        public (float X, float Y) Camera = (0, 0);
        public Player player;
        private (int X, int Y) PlayerTile { get => ((int)(player.GetCenterTiled().X), (int)(player.GetCenterTiled().Y)); }
        private (int X, int Y) PlayerTileCamera { get => ((int)(player.GetCenterTiled().X + Camera.X), (int)(player.GetCenterTiled().Y + Camera.Y)); }
        private Rectangle CameraBounds;
        private SFML.Graphics.Font font;
        private Text text;
        private _Main zelecxModule = null;

        public MainForm()
        {
            InitializeComponent();
            ClientSize = new Size(512, 256);
            Render = new RenderWindow(RenderPanel.Handle);
            string levelfilename = "Levels\\Start.level";
            level = Level.Load(levelfilename);
            level.PlayMode = true;
            level.player = player;
            SpriteManager.Initialize();
            Camera = level.InGameCamera;
            CameraBounds = new Rectangle(0, 0, Tools.MapWidth - Tools.PlayRenderWidth, Tools.MapHeight - Tools.PlayRenderHeight);
            font = new SFML.Graphics.Font("arial.ttf");
            text = new Text("", font, 16);
            text.Color = sfColor.White;

            FormClosed += delegate { timer1.Stop(); timerUpdate.Stop(); SpriteManager.Uninitialize(); KillInstance(); };

            #region zelecx
            if (File.Exists(levelfilename + ".MAP"))
            {
                zelecxModule = new _Main(true, new Size(Tools.MapWidth, Tools.MapHeight), Tools.TileSize, RenderPanel);
                zelecxModule.LoadMap(levelfilename + ".MAP");
            }
            #endregion

            timer1.Start();
            timerUpdate.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
            Render.DispatchEvents();

            for (int i = 0; i < 2; i++)
            {
                Render.Clear();

                level.DrawBackground(Render, ((int, int))Camera, true);
                level.Draw(Render, Camera, new bool[] { true, true, true }, false, true);

                if (player != null)
                    player.Draw(Render, Camera);

                DrawGUI();

                Render.Display();
            }
        }
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            level.player = player;

            if (player != null)
            {
                player.Inputs(Render, level);
                CheckPlayerWarps();
                if (level.InGameCamera != ((int)Camera.X, (int)Camera.Y))
                    Camera = level.InGameCamera;
                else if(player.IsInMovement())
                {
                    Camera = Framework.Maths.ClampPositionInRectangle((player.GetCenterTiled().X - Tools.PlayRenderWidth / 2F, player.GetCenterTiled().Y - Tools.PlayRenderHeight / 2F), CameraBounds);
                    level.InGameCamera = ((int)Camera.X, (int)Camera.Y);
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                    Camera.X--;
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    Camera.X++;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                    Camera.Y--;
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    Camera.Y++;
            }

            level.UpdateZelecx(zelecxModule);

            level.UpdateVolatiles(Render);
        }
        private void DrawGUI()
        {
            var sp = SpriteManager.GetSpriteGUI(0);
            sp.Position = new Vector2f(5, 5);
            Render.Draw(sp);
            text.DisplayedString = player == null ? "0" : player.HP.ToString();
            text.Position = new Vector2f(25, 5);
            Render.Draw(text);

            sp = SpriteManager.GetSpriteGUI(1);
            sp.Position = new Vector2f(45, 5);
            Render.Draw(sp);
            text.DisplayedString = "0";
            text.Position = new Vector2f(65, 5);
            Render.Draw(text);
        }

        private void RenderPanel_MouseClick(object sender, MouseEventArgs e)
        {
            var info = level.GetTileInfosFromMouse(Render, ((int, int))Camera);
            EntityProperties entity;

            for (int l = 0; l < 3; l++)
            {
                entity = Tools.GetEntityPropertyFromID(info.value[1]);

                if (entity != null)
                {
                    if (entity.EntityPath == Player.GetEntityPath())
                    {
                        if (player != null)
                            level.SetTile(1, PlayerTile.X, PlayerTile.Y, player.ID, false);

                        player = Player.Load(entity);
                        player.position = (info.x * Tools.TileSize, info.y * Tools.TileSize);
                        level.DeleteTile(1, info.x, info.y, false);

                        Camera = Framework.Maths.ClampPositionInRectangle((player.GetCenterTiled().X - Tools.PlayRenderWidth / 2F, player.GetCenterTiled().Y - Tools.PlayRenderHeight / 2F), CameraBounds);
                        level.InGameCamera = ((int)Camera.X, (int)Camera.Y);
                    }
                }
            }

            for (int i = 0; i < 3; i++)
                level.DeleteTile(i, info.x, info.y, false);
        }
        private void CheckPlayerWarps()
        {
            var point = new Point((int)player.GetCenter().X, (int)player.GetCenter().Y);
            point.X = point.X / Tools.TileSize + (int)Camera.X;
            point.Y = point.Y / Tools.TileSize + (int)Camera.Y;

            var warp = level.GetWarp(point.X, point.Y);
            if (warp != null)
            {
                if (warp.type == Warp.WarpType.Exit)
                {
                    level = Level.Load("Levels\\" + warp.Exit_LevelName + ".level");
                    var EnterPos = level.GetWarp(warp.Exit_LevelEnterID).TilePosition;
                    player.position = (EnterPos.X * Tools.TileSize, EnterPos.Y * Tools.TileSize);
                }
            }
        }

        private (int W, int H) GetRenderSize()
        {
            return ((int)Render.Size.X, (int)Render.Size.Y);
        }
        private (int X, int Y) GetCamera()
        {
            return ((int)Camera.X, (int)Camera.Y);
        }
    }
}
