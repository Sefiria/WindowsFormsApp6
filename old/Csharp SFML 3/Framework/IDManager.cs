using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class IDManager
    {
        static public List<(byte layer, byte ID)> DoorsID;

        public static void Initialize()
        {
            AnalyseDoors();
        }
        public static void Dispose()
        {
            DoorsID.Clear();
        }

        private static void AnalyseDoors()
        {
            DoorsID = new List<(byte layer, byte ID)>();
            var doors = Tools.GetSubFolderEntities("Door");
            foreach(var door in doors)
                DoorsID.Add((door.layer, door.ID));
        }
    }
}
