﻿using console_v2.res.entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using Tooling;

namespace console_v2
{
    public class SceneAdventure : Scene
    {
        public static Rectangle DrawingRect => new RectangleF(Core.Instance.ScreenWidth / 2 - Chunk.ChunkSize.x * GraphicsManager.CharSize.Width / 2f,
                                                                                              Core.Instance.ScreenHeight / 2 - Chunk.ChunkSize.y * GraphicsManager.CharSize.Height / 2f,
                                                                                              Chunk.ChunkSize.x * GraphicsManager.CharSize.Width,
                                                                                              Chunk.ChunkSize.y * GraphicsManager.CharSize.Height).ToIntRect();

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

                if (KB.IsKeyPressed(KB.Key.Tab)) Core.Instance.SwitchScene(Core.Scenes.Craft, 1);
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
            float sz = 10 + GraphicsManager.CharSize.Width + 10 + 10;
            float total_sz = shortcuts.Count * GraphicsManager.CharSize.Width + 20;
            float _x = DrawingRect.X + DrawingRect.Width / 2 - total_sz / 2;
            SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.Gray));

            for (int i = 0; i < shortcuts.Count; i++)
            {
                int x = (int)(_x + i * sz);
                gui.FillRectangle(brush, x, 50, sz, GraphicsManager.CharSize.Height + 10);

                var dbref = Core.Instance.TheGuy.Inventory.GetDBRefByUniqueId(shortcuts[i].Ref.UniqueId);
                (int CharToDisplay, Bitmap DBResSpe) = DB.RetrieveDBResOrSpe(dbref);
                if(CharToDisplay > -1)
                    gui.DrawString(string.Concat((char)CharToDisplay), GraphicsManager.FontSQ, shortcuts[i].IsDown() ? Brushes.White : Brushes.Gray, x + 10, 50);
                else
                    gui.DrawImage(DBResSpe, x + 10, 50);
            }
        }


        private void ItemToPlaceUpdate()
        {
            if (MouseStates.IsButtonPressed(System.Windows.Forms.MouseButtons.Left))
            {
                var tile = (MouseStates.Position.vecf() - DrawingRect.Location.vecf()).ToTile();
                Core.Instance.TheGuy.Inventory.RemoveOne(ItemToPlace.DBRef);
                new EntityStructure(tile, (Structures)ItemToPlace.DBRef);
                ItemToPlace = null;
            }
        }
        public void ItemToPlaceDraw(Graphics g, Graphics gui)
        {
            var (CharToDisplay, DBResSpe) = DB.RetrieveDBResOrSpe(ItemToPlace.DBRef);
            var Position = (MouseStates.Position.vecf() - DrawingRect.Location.vecf()).ToTile().ToWorld();
            if (CharToDisplay != -1)
                GraphicsManager.DrawString(g, string.Concat((char)CharToDisplay), new SolidBrush(Color.FromArgb(100, Color.White)), Position);
            else if (DBResSpe != null)
                GraphicsManager.DrawImage(g, DBResSpe.WithOpacity(100f/byte.MaxValue), Position);
        }
    }
}
