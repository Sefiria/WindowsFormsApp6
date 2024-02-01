﻿using System;
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
        internal int HandObject = -1;//Map.Events index

        public Character(string filename, float x, float y) : base(filename, x, y){ Initialize(); }
        public Character(Bitmap image, float x, float y) : base(image, x, y){ Initialize(); }

        internal void Initialize()
        {
            Offset = (- Core.TileSize / 2F-5F, -5F).P();
            OffsetTexture.X = -Core.TileSize / 2 - 5;
            OffsetTexture.Y = - 5;
        }
        internal override void Update()
        {
            update_controls();
            base.Update();
        }
        private void update_controls()
        {
            if (HandObject > -1) Map.Current.Events[HandObject].Layer = EvLayer.Below;

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

            if (HandObject > -1)
            {
                var ts = Core.TileSize;
                var hand = Map.Current.Events[HandObject];
                hand.Layer = EvLayer.Above;
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
                Map.Current.Events[HandObject] = hand;
            }
        }
    }
}
