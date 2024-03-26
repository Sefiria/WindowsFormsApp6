using Core.Behaviors.Skills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static _Console.Core.Utils.Maths;
using static Core.Utils.Inputs;

namespace Core.Entities
{
    public class Player : Ship
    {
        public Armament armamentLeft, armamentRight;
        public float MoveSpeed = 1.2F;

        public Player(Vec Position = null) : base("PlayerShip")
        {
            armamentLeft = new ArmamentSimple();
            armamentRight = new ArmamentSimple();
            
            this.Position = Position != null ? Position + Center / 2 : Vec.Zero;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            armamentLeft.Render(g);
            armamentRight.Render(g);
        }
        public override void Update()
        {
            if (KeyDown(Key.Z))         Position.Y -= MoveSpeed;
            else if (KeyDown(Key.Q))    Position.Y += MoveSpeed;
            if (KeyDown(Key.S))         Position.X -= MoveSpeed;
            else if(KeyDown(Key.D))     Position.X += MoveSpeed;

            armamentLeft.Position = Position + new Vec(10, 1);
            armamentRight.Position = Position + new Vec(10, 11);
        }
    }
}
