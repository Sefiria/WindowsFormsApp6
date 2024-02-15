using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Tooling;
using WindowsFormsApp24.Events;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24
{
    internal class Character : Event
    {
        internal float LookX, LookY;
        internal Guid HandObject = Guid.Empty;
        internal bool HandObjectDefined => HandObject.IsDefined();
        internal Event HandEvent => Map.GetEvent(HandObject);

        internal static Guid MainHandObject => Core.MainCharacter.HandObject;
        internal static bool MainHandObjectDefined => MainHandObject.IsDefined();
        internal static Event MainHandEvent => Map.GetEvent(MainHandObject);

        public Character(string filename, int x, int y, int z) : base(filename, x, y, z){ Initialize(); }
        public Character(string filename, float x, float y, float z) : base(filename, x, y, z){ Initialize(); }
        public Character(Bitmap image, int x, int y, int z) : base(image, x, y, z){ Initialize(); }
        public Character(Bitmap image, float x, float y, float z) : base(image, x, y, z){ Initialize(); }

        internal void Initialize()
        {
            TextureOffset.X = -Core.TileSize / 2F - 5F;
            TextureOffset.Y = -5F;
            Bounds = new RectangleF(TextureOffset.X+8, TextureOffset.Y, Core.TileSize, Core.TileSize / 2F);
        }
        internal override void Update()
        {
            update_controls();
            base.Update();
        }
        private void update_controls()
        {
            bool up = Core.GetInput(InputNames.Up);
            bool left = Core.GetInput(InputNames.Left);
            bool down = Core.GetInput(InputNames.Down);
            bool right = Core.GetInput(InputNames.Right);

            IsMoving = up || left || down || right;

            if (IsMoving)
            {
                LookX = 0; LookY = 0;
                if (left) LookX -= 1;
                if (right) LookX += 1;
                if (down) LookY += 1;
                if (up) LookY -= 1;
                Direction = LookY < 0 ? 3 : (LookY > 0 ? 0 : (LookX < 0 ? 1 : 2));
                if(MoveSpeed > 0F) Move();
                MoveSpeed += 0.8F;
                if (MoveSpeed > MaxSpeed) MoveSpeed = MaxSpeed;
            }
            else
            {
                if (MoveSpeed > 0F) MoveSpeed -= 0.15F;
                if (MoveSpeed <= 0F) { MoveSpeed = 0F; LookX = 0; LookY = 0; }
                else Move();
                if(MoveSpeed == 0F && !DoesntCollides(0F, 0F))// character is blocked !
                {
                    bool unstucked = false;
                    int ts = Core.TileSize;
                    int d = 1;
                    while(unstucked == false)
                    {
                        /* */if (DoesntCollides(-1F * d * ts,  0F * d * ts)) { unstucked = true; X -=  1F * d * ts; }
                        else if (DoesntCollides( 0F * d * ts, -1F * d * ts)) { unstucked = true; Y -=  1F * d * ts; }
                        else if (DoesntCollides( 1F * d * ts,  0F * d * ts)) { unstucked = true; X += 1F * d * ts; }
                        else if (DoesntCollides( 0F * d * ts,  1F * d * ts)) { unstucked = true; Y += 1F * d * ts; }
                        else d++;
                    }
                }
            }
            void Move()
            {
                IsMoving = true;
                if (LookX < 0 && DoesntCollides(-MoveSpeed, 0)) X -= MoveSpeed;
                if (LookX > 0 && DoesntCollides(MoveSpeed, 0)) X += MoveSpeed;
                if (LookY > 0 && DoesntCollides(0, MoveSpeed)) Y += MoveSpeed;
                if (LookY < 0 && DoesntCollides(0, -MoveSpeed)) Y -= MoveSpeed;
            }

            if(IsMoving) Map.Current.complete_refresh = true;

            if (HandObjectDefined)
            {
                var ts = Core.TileSize;
                var hand = HandEvent;
                int w = hand.W;
                int hw = w / 2;
                int h = hand.H;
                int hh = h / 2;
                switch (Direction)
                {
                    case 0: hand.X = X - hw; hand.Y = Y - hh + h*0.75F; break;// ↓
                    case 1: hand.X = X - hw - ts / 2; hand.Y = Y - hh + h * 0.25F; break;// ←
                    case 2: hand.X = X - hw + ts * 0.7F; hand.Y = Y - hh + h * 0.25F; break;// →
                    case 3: hand.X = X - hw; hand.Y = Y - hh - h / 2; hand.Layer = EvLayer.Below; break;// ↑
                }
                Map.SetEvent(HandObject, hand);
            }
        }
    }
}
