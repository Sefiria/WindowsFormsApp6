using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Material
{
    public class VolatileBlock : Material
    {
        public (float X, float Y) velocity = (0F, 0F);
        public float MaxVelocity = 3F;
        public float speed = 0.1F;
        public byte Layer;
        public (byte layer, byte id) FutureStaticBlock = (0, 0);

        public VolatileBlock(int tiledX, int tiledY, byte layer, byte id)
        {
            position = (tiledX * Tools.TileSize, tiledY * Tools.TileSize);
            Layer = layer;
            ID = id;
            FutureStaticBlock = (layer, id);
        }

        public new static string GetEntityPath()
        {
            return Material.GetEntityPath() + "/Block";
        }

        public void Draw(RenderWindow Render, (float X, float Y) Camera)
        {
            var sp = SpriteManager.GetSprite(Layer, ID);
            sp.Position = new Vector2f((int)GetCenter().X - Camera.X * Tools.TileSize, (int)GetCenter().Y - Camera.Y * Tools.TileSize);
            sp.Origin = new Vector2f(Tools.TileSize / 2F, Tools.TileSize / 2F);
            Render.Draw(sp);
            sp.Origin = new Vector2f(0, 0);
        }

        public void Update(Level level, RenderWindow Render)
        {
            velocity.Y += speed * 2;
            if (velocity.Y > MaxVelocity)
                velocity.Y = MaxVelocity;
            if (level.CheckCollision(Level.CollisionTest.Bottom, position, Render))
            {
                level.SetTile(FutureStaticBlock.layer, new Point((int)(position.X / Tools.TileSize), (int)position.Y / Tools.TileSize), FutureStaticBlock.id, false);
                level.volatiles.Remove(this);
                return;
            }
            position.Y += velocity.Y;
        }
    }
}
