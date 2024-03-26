using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities
{
    [Serializable]
    public class Entity
    {
        public (float X, float Y) position;
        public string Name = "";
        public byte ID;
        public bool ScriptEnabled;

        public Entity()
        {
        }

        static public string GetEntityPath()
        {
            return "Entities/Entity";
        }

        public Point GetPositionAsPoint()
        {
            return new Point((int)position.X, (int)position.Y);
        }
        public (float X, float Y) GetCenter()
        {
            return (position.X + Tools.TileSize / 2F, position.Y + Tools.TileSize / 2F);
        }
        public (float X, float Y) GetCenterTiled()
        {
            return (position.X / Tools.TileSize + 0.5F, position.Y / Tools.TileSize + 0.5F);
        }
    }
}
