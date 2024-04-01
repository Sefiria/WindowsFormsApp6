using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;
using Tooling;

namespace console_v2
{
    public class SceneAdventure : Scene
    {
        public World World;
        public static Rectangle DrawingRect => new RectangleF(Core.Instance.ScreenWidth / 2 - Chunk.ChunkSize.x * GraphicsManager.CharSize.Width / 2f,
                                                                                              Core.Instance.ScreenHeight / 2 - Chunk.ChunkSize.y * GraphicsManager.CharSize.Height / 2f,
                                                                                              Chunk.ChunkSize.x * GraphicsManager.CharSize.Width,
                                                                                              Chunk.ChunkSize.y * GraphicsManager.CharSize.Height).ToIntRect();

        public SceneAdventure() : base()
        {
            World = new World();
            var index = (0, 0).V();
            World.Dimensions[index] = new Dimension(index);
        }
        public override void Initialize()
        {
            BGColor = Color.Black;
        }

        public override void Update()
        {
            base.Update();

            World.Update();
            NotificationsManager.Update();

            update_shortcuts();

            if (KB.IsKeyPressed(KB.Key.Escape)) Core.Instance.SwitchScene(Core.Scenes.Menu);
        }

        public override void TickSecond()
        {
            World.TickSecond();
        }

        public override void Draw(Graphics g, Graphics gui)
        {
            base.Draw(g, gui);

            World.Draw(g, gui);
            NotificationsManager.Draw(gui);

            draw_shortcuts(gui);

            gui.DrawRectangle(new Pen(Color.FromArgb(100, 100, 0)), new Rectangle(DrawingRect.Location, (Size)((Point)DrawingRect.Size).Minus(1)));
        }

        private void update_shortcuts()
        {
            Core.Instance.Shortcuts.ForEach(s =>
            {
                if (KB.IsKeyPressed(s.Key))
                {
                    (s.Ref as IItem)?.Consume();
                    (s.Ref as ITool)?.Use(Core.Instance.TheGuy);
                }
            });
        }
        private void draw_shortcuts(Graphics gui)
        {
            var shortcuts = Core.Instance.Shortcuts;
            float sz = 10 + GraphicsManager.CharSize.Width + 10 + 10;
            float total_sz = shortcuts.Count * GraphicsManager.CharSize.Width + 20;
            float _x = DrawingRect.X + DrawingRect.Width / 2 - total_sz / 2;
            SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.Gray));
            for(int i = 0; i < shortcuts.Count; i++)
            {
                int x = (int)(_x + i * sz);
                gui.FillRectangle(brush, , 50, sz, GraphicsManager.CharSize.Height + 10);

                int r = shortcuts[i].Ref.UniqueId;// TODO if item or tool, retrieve dbref
                // TODO char or bitmap ?
                if (DB.Resources.ContainsKey(dbref))
                    CharToDisplay = DB.Resources[dbref];
                else if (DB.ResourcesSpecials.ContainsKey(dbref))
                    DBResSpe = dbref;
                // TODO draw char or bitmap (res or spe)
                gui.DrawString(DB. )
            }
        }
    }
}
