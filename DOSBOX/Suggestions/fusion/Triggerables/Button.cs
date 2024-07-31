using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX.Suggestions.fusion.Triggerables
{
    public class Button : PhysicalObject, ITriggerable
    {
        public Enumerations.PhysicalObjectSide Side;
        public bool Pushed;
        public List<vec> Linked_tiles = new List<vec>();
        public string BaseHash { get; set; } = null;
        public string Hash = null;

        public Button(byte room_id, RoomData_objects po) : base(po)
        {
            vec = po.vec.AsVec().f * Tile.TSZ;
            Side = po.side;
            if (po.data.TryGetValue("linked", out var linkedValue) && linkedValue is JsonElement linkedElement && linkedElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement element in linkedElement.EnumerateArray())
                {
                    if (element.TryGetProperty("x", out JsonElement xElement) && element.TryGetProperty("y", out JsonElement yElement))
                    {
                        int x = xElement.GetInt32();
                        int y = yElement.GetInt32();
                        Linked_tiles.Add(new vec(x, y));
                    }
                }
            }

            BaseHash = GenerateBaseHash(room_id, po.vec.x, po.vec.y);
            Hash = GenerateHash(room_id, po.vec.x, po.vec.y);

            Pushed = Register.triggered_objects.Contains(BaseHash) ? true : po.data.TryGetValue("pushed", out var pushedValue) && pushedValue.ToString().ToLower() == "true";

            CreateGraphics();
        }

        private void CreateGraphics()
        {
            byte[,] human_readable = null;
            if (Pushed)
            {
                switch (Side)
                {
                    case Enumerations.PhysicalObjectSide.Top:
                        human_readable = new byte[2, 8]
                        {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 0, 3, 3, 3, 3, 3, 3, 0 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Bottom:
                        human_readable = new byte[2, 8]
                        {
                        { 0, 3, 3, 3, 3, 3, 3, 0 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Left:
                        human_readable = new byte[8, 2]
                        {
                        { 3,0 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,0 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Right:
                        human_readable = new byte[8, 2]
                        {
                        { 0,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 3,3 },
                        { 0,3 }
                        }; break;
                }
            }
            else
            {
                switch (Side)
                {
                    case Enumerations.PhysicalObjectSide.Top:
                        human_readable = new byte[3, 8]
                        {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 2, 2, 2, 1, 1, 2, 3 },
                        { 0, 3, 3, 3, 3, 3, 3, 0 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Bottom:
                        human_readable = new byte[3, 8]
                        {
                        { 0, 3, 3, 3, 3, 3, 3, 0 },
                        { 3, 2, 2, 2, 1, 1, 2, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Left:
                        human_readable = new byte[8, 3]
                        {
                        { 3, 3, 0 },
                        { 3, 1, 3 },
                        { 3, 0, 3 },
                        { 3, 1, 3 },
                        { 3, 2, 3 },
                        { 3, 2, 3 },
                        { 3, 2, 3 },
                        { 3, 3, 0 }
                        }; break;
                    case Enumerations.PhysicalObjectSide.Right:
                        human_readable = new byte[8, 3]
                        {
                        { 0, 3, 3 },
                        { 3, 1, 3 },
                        { 3, 0, 3 },
                        { 3, 1, 3 },
                        { 3, 2, 3 },
                        { 3, 2, 3 },
                        { 3, 2, 3 },
                        { 0, 3, 3 }
                        }; break;
                }
            }

            var w = human_readable.GetLength(1);
            var h = human_readable.GetLength(0);
            g = new byte[w, h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    g[x, y] = human_readable[y, w-1-x];

            scale = 1;
        }

        public void Trigger(Dispf by)
        {
            if(!Pushed)
            {
                Pushed = true;
                Register.Write(this);
                CreateGraphics();

                var room = Fusion.Instance.room;
                foreach (var tile in Linked_tiles)
                {
                    var door = room.Doors.FirstOrDefault(d => (d.vec / Tile.TSZ).i == tile && d.state != 2);
                    if (door != null)
                    {
                        door.state = 22;
                        door.Locked = false;
                        Register.Write(door);
                    }
                }
            }
        }
        public static string GenerateBaseHash(byte room_id, int x, int y) => $"obj-{room_id}-{x}-{y}";
        public static string GenerateHash(byte room_id, int x, int y) => GenerateBaseHash(room_id, x, y) + "+" + RandomThings.GetCurrentTickDigits(4);
    }
}
