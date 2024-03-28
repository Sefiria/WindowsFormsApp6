using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
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

            if (KB.IsKeyPressed(KB.Key.Escape)) Core.Instance.SwitchScene(Core.Scenes.Menu);
        }

        public override void TickSecond()
        {
            World.TickSecond();
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            World.Draw(g);
            NotificationsManager.Draw(Core.Instance.gui);

            Core.Instance.gui.DrawRectangle(new Pen(Color.FromArgb(100, 100, 0)), new Rectangle(DrawingRect.Location, (Size)((Point)DrawingRect.Size).Minus(1)));
        }
    }
}
