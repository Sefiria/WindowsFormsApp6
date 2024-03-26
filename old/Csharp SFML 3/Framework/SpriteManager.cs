using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework
{
    public static class SpriteManager
    {
        static Dictionary<(byte layer, byte id), Sprite> sprites = new Dictionary<(byte layer, byte id), Sprite>();
        static List<(byte layer, byte id)> displayedExceptions = new List<(byte layer, byte id)>();
        static Sprite WarpEnterSprite, WarpExitSprite;
        static List<(byte layer, byte id, List<Sprite> sprites)> SpritesAutoTiles = new List<(byte layer, byte id, List<Sprite>)>();
        static List<Sprite> SpritesGUI = new List<Sprite>();

        public static void Initialize()
        {
            // sprites

            var entities = Tools.GetAllEntities();

            foreach (var entity in entities)
            {
                var key = (entity.layer, entity.ID);
                entity.image.CreateMaskFromColor(new SFML.Graphics.Color(entity.TransparentColor.R, entity.TransparentColor.G, entity.TransparentColor.B));
                if (!sprites.ContainsKey(key))
                {
                    sprites[key] = new Sprite(new Texture(entity.image));
                    if (entity.autoTiled)
                        SpritesAutoTiles.Add((entity.layer, entity.ID, entity.GetAutoTilesRawsSpriteList()));
                }
                else
                    MessageBox.Show("The entity with ID '" + (int)entity.ID + "' already exists (ID conflict) ; Please check the ID Conflicts Manager");
            }

            var img = Tools.BitmapToSfImage(Properties.Resources.WarpEnter);
            img.CreateMaskFromColor(SFML.Graphics.Color.White);
            WarpEnterSprite = new Sprite(new Texture(img));

            img = Tools.BitmapToSfImage(Properties.Resources.WarpExit);
            img.CreateMaskFromColor(SFML.Graphics.Color.White);
            WarpExitSprite = new Sprite(new Texture(img));

            var listBmp = Tools.SplitBitmap(Properties.Resources.GUI, 16);
            foreach (var bmp in listBmp)
                SpritesGUI.Add(new Sprite(new Texture(Tools.BitmapToSfImage(bmp))));
        }
        public static void Uninitialize()
        {
            SpritesAutoTiles.Clear();
            SpritesGUI.Clear();
            sprites.Clear();
            displayedExceptions.Clear();
        }
        public static void Dispose()
        {
            sprites.Clear();
            displayedExceptions.Clear();
        }
        public static Sprite GetSprite(byte layer, byte id)
        {
            if (sprites.ContainsKey((layer, id)))
                return sprites[(layer, id)];

            if (!displayedExceptions.Contains((layer, id)))
            {
                displayedExceptions.Add((layer, id));
                MessageBox.Show("The entity object [" + (int)layer + "," + (int)id + "] was not found ; Check maybe the ID Conflicts Manager");
            }

            return null;
        }
        public static Sprite GetSpritesAutoTile(byte layer, byte id, int microtile)
        {
            var found = SpritesAutoTiles.FirstOrDefault(x => x.layer == layer && x.id == id);
            return found.Equals(default) ? null : found.sprites[microtile];
        }
        public static Sprite GetWarpSprite(Warp.WarpType type)
        {
            if (type == Warp.WarpType.Enter)
                return WarpEnterSprite;
            else
                return WarpExitSprite;
        }
        public static Sprite GetSpriteGUI(int id)
        {
            if (id < 0 || id >= SpritesGUI.Count)
                return null;
            return SpritesGUI[id];
        }
    }
}
