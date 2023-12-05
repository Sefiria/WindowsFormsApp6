using ConfigureRoute.Entities;
using ConfigureRoute.Obj;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using static ConfigureRoute.Enumerations;

namespace ConfigureRoute
{
    public static class InternalTools
    {
        public static PointF AsLook(this int direction)
        {
            if (direction < 0 || direction > 3) return PointF.Empty;
            switch(direction)
            {
                default: return PointF.Empty;
                case UP: return new PointF(0, -1);
                case LEFT: return new PointF(-1, 0);
                case DOWN: return new PointF(0, 1);
                case RIGHT: return new PointF(1, 0);
            }
        }
        public static int AsDirection(this PointF lookf)
        {
            Point look = lookf.Round().ToPoint();
            if (look.X == 0 && look.Y == 0) return -1;
            if (look.X != 0 && look.Y != 0) return -1;
            if (look.Y < 0) return UP;
            if (look.X < 0) return LEFT;
            if (look.Y > 0) return DOWN;
            if (look.X > 0) return RIGHT;
            return -1;
        }
        public static int ReverseDirection(this int direction)
        {
            switch (direction)
            {
                default: return direction;
                case 0: return 2;
                case 1: return 3;
                case 2: return 0;
                case 3: return 1;
            }
        }
        public static int AsReversedDirection(this PointF lookf) => lookf.AsDirection().ReverseDirection();
        public static Ways AsWay(this int Direction, PointF lookf)
        {
            Point look = lookf.Round().ToPoint();
            if (look.X == 0 || look.Y == 0) return Ways.Devant;
            if (look.X != 0 || look.Y != 0) return Ways.Devant;
            var v = new LoopValue(0, 0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (Direction == i)
                {
                    v.Value = Direction + i;
                    return (Ways)v.Value;
                }
            }
            return Ways.Devant;
        }
        public static int GetSignValueFromWay(this Road road, Ways way)
        {
            int? t = road.Sign?.t;
            int? l = road.Sign?.l;
            int? b = Core.Map.FindRoad(road.Position.x, road.Position.y + 1)?.Sign?.t;
            int? r = Core.Map.FindRoad(road.Position.x + 1, road.Position.y)?.Sign?.l;

            var v = new LoopValue(0, 0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (road.Direction == i)
                {
                    v.Value = road.Direction + (int)way;
                    if (v.Value == UP) return t ?? 0;
                    if (v.Value == LEFT) return l ?? 0;
                    if (v.Value == DOWN) return b ?? 0;
                    if (v.Value == RIGHT) return r ?? 0;
                }
            }
            return 0;
        }
        public static int RotateDirection(this int direction, Ways turn_direction)
        {
            var v = new LoopValue(direction, 0, 3);
            switch (turn_direction)
            {
                case Ways.Devant: return direction;
                case Ways.Derriere: return direction.ReverseDirection();
                case Ways.AGauche: v.Value++; return v.Value;
                case Ways.ADroite: v.Value--; return v.Value;
            }
            return direction;
        }
        public static bool ContainedByRoad(this Car car, Road road) => car.Edges.Any(pt => road.Bounds.Contains(pt));
    }
}
