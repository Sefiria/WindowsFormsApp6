using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;

namespace Framework.Entities._Entity._Organic._Playable
{
    public class Player : Playable
    {
        Sprite[] anim = new Sprite[3];
        float animIT = 0F;
        (float X, float Y) velocity = (0F, 0F);
        float MaxVelocity = 3F;
        float speed = 0.1F;
        bool flipDraw = false;
        bool Space_R = true;

        public Player()
        {

        }

        public new static string GetEntityPath()
        {
            return Playable.GetEntityPath() + "/Player";
        }

        static public Player Load(EntityProperties prop)
        {
            Player player = new Player();
            for (int i = 0; i<3; i++)
            {
                sfImage img = prop.AutoTilesRaws[i];
                img.CreateMaskFromColor(new sfColor(prop.TransparentColor.R, prop.TransparentColor.G, prop.TransparentColor.B));
                player.anim[i] = new Sprite(new Texture(img));
            }

            player.ID = prop.ID;
            player.HP = prop.HP;
            player.indestrucible = prop.indestructible;

            return player;
        }
        public void Draw(RenderWindow Render, (float X, float Y) Camera)
        {
            var sp = GetAnimCurrentSprite();
            sp.Position = new Vector2f((int)GetCenter().X - Camera.X * Tools.TileSize, (int)GetCenter().Y - Camera.Y * Tools.TileSize);
            sp.Origin = new Vector2f(Tools.TileSize / 2F, Tools.TileSize / 2F);
            sp.Scale = new Vector2f(flipDraw ? -1 : 1, 1);
            Render.Draw(sp);
        }
        public Sprite GetAnimCurrentSprite()
        {
            return anim[(int)animIT];
        }
        public void Inputs(RenderWindow Render, Level level)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (velocity.X < 0F)
                    velocity.X /= 1.1F;
                velocity.X += speed;
                flipDraw = false;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                if (velocity.X > 0F)
                    velocity.X /= 1.1F;
                velocity.X -= speed;
                flipDraw = true;
            }
            if (!Keyboard.IsKeyPressed(Keyboard.Key.Q) && !Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                velocity.X /= 1.1F;
                animIT = 1F;
            }

            if (velocity.X < -MaxVelocity) velocity.X = -MaxVelocity;
            if (velocity.X > MaxVelocity) velocity.X = MaxVelocity;
            if (level.CheckCollision(velocity.X > 0F ? Level.CollisionTest.Right : Level.CollisionTest.Left, position, Render))
            {
                position.X -= velocity.X;
                velocity.X = 0F;
                animIT = 1F;
            }
            else
                position.X += velocity.X;
            animIT += (velocity.X < 0F ? -velocity.X : velocity.X) / 10F;
            if (animIT >= 3F) animIT -= 3F;





            velocity.Y += speed * 2;
            if (level.CheckCollision(Level.CollisionTest.Bottom, position, Render))
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && Space_R)
                    velocity.Y -= speed * 40F;
                else
                {
                    velocity.Y = 0F;
                    position.Y = Tools.Snap((int)position.Y);
                }
            }
            else
            {
                if (level.CheckCollision(Level.CollisionTest.Top, position, Render))
                {
                    position.Y += velocity.Y;
                    velocity.Y = speed;

                    position.Y += velocity.Y;
                    if (level.CheckCollision(Level.CollisionTest.Bottom, position, Render))
                        velocity.Y = 0F;
                    position.Y -= velocity.Y;
                }
            }
            position.Y += velocity.Y;

            Space_R = !Keyboard.IsKeyPressed(Keyboard.Key.Space);
        }
        public bool IsInMovement()
        {
            return velocity.X < -0.001F || velocity.X > 0.001F || velocity.Y < -0.001F || velocity.Y > 0.001F;
        }

    }
}
