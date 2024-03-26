using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Material
{
    [Serializable]
    public class Door : Material
    {
        public Door Exit = null;
        public bool Locked = false;
        public byte Layer = 0;
        public byte ID = 0;

        public new static string GetEntityPath()
        {
            return Material.GetEntityPath() + "/Door";
        }

        public Door(byte _layer = 0, byte _ID = 0, Door _exit = null)
        {
            Name = "Unnamed";
            Layer = _layer;
            ID = _ID;
            Exit = _exit;
        }

        static public Door FirstOrDefault(List<Door> list, byte layer, int x, int y, byte ID)
        {
            return list.FirstOrDefault(i => i.Layer == layer && i.position.X == x && i.position.Y == y && i.ID == ID);
        }

        public override string ToString()
        {
            return Name + ", " + Layer + ", " + ID + ", " + position.X + ", " + position.Y;
        }
    }
}
