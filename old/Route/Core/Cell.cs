using Script;
using System;
using System.Drawing;

namespace Core
{
    public class Cell
    {
        [ScriptProperty]
        public int X { get; set; }
        [ScriptProperty]
        public int Y { get; set; }
        [ScriptProperty]
        public bool ContainsEntity => entity != null;

        public Tile tile = null;
        public Entity entity;

        public Point Location => new Point(X, Y);


        public Cell(int X, int Y, Entity entity = null)
        {
            this.X = X;
            this.Y = Y;
            this.entity = entity;
        }
        public (bool, string) Set(params object[] args) => entity?.Set(args) ?? (true, "");
    }
}
