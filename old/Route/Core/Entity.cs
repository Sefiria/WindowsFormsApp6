using System;
using System.Drawing;

namespace Core
{
    public class Entity
    {
        public enum RelativeCell { Left = 0, Right, Up, Down }

        public RelativeCell NextCell { get; set; }
        public int X, Y;
        public Point Location => new Point(X, Y);

        public Entity()
        {
        }
        public (bool, string) Set(params object[] args)
        {
            try
            {
                object[] p = (object[])args[0];
                string arg = (string)p[0];

                if (arg != "@")
                {
                    RelativeCell val;
                    if(Enum.TryParse(arg, out val) == false)
                    {
                        return (false, $"Set function : unknown argument '{arg}'");
                    }
                    else
                    {
                        NextCell = val;
                    }
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

            return (true, "");
        }

        public override string ToString()
        {
            return NextCell.ToString();
        }
    }
}
