using console_v3.res.entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using Tooling;

namespace console_v3
{
    public class SceneAdventure : Scene
    {
        public static Rectangle DrawingRect => new RectangleF(Core.Instance.ScreenWidth / 2 - Chunk.ChunkSize.x * GraphicsManager.TileSize / 2f,
                                                                                              Core.Instance.ScreenHeight / 2 - Chunk.ChunkSize.y * GraphicsManager.TileSize / 2f,
                                                                                              Chunk.ChunkSize.x * GraphicsManager.TileSize,
                                                                                              Chunk.ChunkSize.y * GraphicsManager.TileSize).ToIntRect();

        public World World;
        public Item ItemToPlace = null;

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

            if (ItemToPlace != null)
            {
                if (KB.IsKeyPressed(KB.Key.Escape))
                {
                    ItemToPlace = null;
                    return;
                }

                ItemToPlaceUpdate();
                NotificationsManager.Update();
            }
            else
            {
                World.Update();
                NotificationsManager.Update();

                update_shortcuts();

                if (KB.IsKeyPressed(KB.Key.Tab)) Core.Instance.SwitchScene(Core.Scenes.Craft, 2);
                if (KB.IsKeyPressed(KB.Key.Escape)) Core.Instance.SwitchScene(Core.Scenes.Menu);
            }
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

            var ms = MouseStates.Position.Minus(DrawingRect.Location);
            var ms_tile = ms.vecf().ToTile();
            World.GetChunk().Entities.Where(e => e.Tile == ms_tile).ToList().ForEach(e => e.DrawHint(gui));

            if (ItemToPlace != null)
                ItemToPlaceDraw(g, gui);
        }

        private void update_shortcuts()
        {
            Core.Instance.Shortcuts?.ForEach(s =>
            {
                if (s.IsPressed())
                {
                    (s.Ref as IItem)?.Consume();
                    (s.Ref as ITool)?.Use(Core.Instance.TheGuy);
                }
            });
        }
        private void draw_shortcuts(Graphics gui)
        {
            var shortcuts = Core.Instance.Shortcuts;
            if (shortcuts == null || shortcuts.Count == 0)
                return;
            float sz = 10 + GraphicsManager.TileSize + 10 + 10;
            float total_sz = shortcuts.Count * GraphicsManager.TileSize + 20;
            float _x = DrawingRect.X + DrawingRect.Width / 2 - total_sz / 2;
            SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.White));

            for (int i = 0; i < shortcuts.Count; i++)
            {
                int x = (int)(_x + i * sz);
                gui.FillRectangle(brush, x, 50, 50, 50);

                var (img, dbref) = Core.Instance.TheGuy.Inventory.GetImageAndDBRefByUniqueId(shortcuts[i].Ref.UniqueId);
                gui.DrawImage(img ?? DB.GetTexture(dbref), x + 10, 60);
            }
        }


        private void ItemToPlaceUpdate()
        {
            if (MouseStates.IsButtonPressed(System.Windows.Forms.MouseButtons.Left))
            {
                var tile = (MouseStates.Position.vecf() - DrawingRect.Location.vecf()).ToTile();
                Core.Instance.TheGuy.Inventory.RemoveOne(ItemToPlace.DBRef);
                new EntityStructure(tile, ItemToPlace.DBRef);
                ItemToPlace = null;
            }
        }
        public void ItemToPlaceDraw(Graphics g, Graphics gui)
        {
            var Position = (MouseStates.Position.vecf() - DrawingRect.Location.vecf()).ToTile().ToWorld();
            GraphicsManager.Draw(g, DB.GetTexture(ItemToPlace.DBRef).WithOpacity(100f/byte.MaxValue), Position);
        }
    }
}
