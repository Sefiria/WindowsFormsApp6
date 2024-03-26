using System;

namespace _Console.Core
{
    public class Map
    {
        public Room[,] Rooms = null;
        public int RoomsCount = 0;
        public int RoomSize = 0;

        public Map() { }
        public Map(int RoomsCount, int RoomSize, bool InstantiateRooms = true)
        {
            this.RoomsCount = RoomsCount;
            this.RoomSize = RoomSize;
            if (InstantiateRooms)
            {
                Rooms = new Room[RoomsCount, RoomsCount];
                for(int x=0; x<RoomsCount; x++)
                {
                    for(int y=0; y< RoomsCount; y++)
                    {
                        Rooms[x, y] = new Room(RoomSize);
                        Rooms[x, y].Initialize(RoomSize);
                    }
                }
            }
        }

        public Room this[int i, int j] { get => Rooms[i, j]; set => Rooms[i, j] = value; }
        public Room this[Vec Loc] { get => Rooms[Loc.x, Loc.y]; set => Rooms[Loc.x, Loc.y] = value; }

        public string GetBuffer(int RoomsCountToDisplay, Vec RoomId, int OffsetX, int OffsetY)
        {
            string buffer = "";

            for (int k = 0; k < OffsetY; k++)
                buffer += '\n';

            for (int ry = RoomId.y; ry < RoomId.y + RoomsCountToDisplay; ry++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    for (int k = 0; k < OffsetX; k++)
                        buffer += ' ';

                    for (int rx = RoomId.x; rx < RoomId.x + RoomsCountToDisplay; rx++)
                    {
                        for (int i = 0; i < RoomSize; i++)
                            buffer += HelpBlock.GetBlockFromId(Rooms[rx, ry][i, j]).RenderChar;
                    }
                }
            }

            return buffer;
        }
        public void DrawBuffer(int RoomsCountToDisplay, Vec RoomId, int OffsetX, int OffsetY)
        {
            for (int k = 0; k < OffsetY; k++)
                Console.Write(' ');

            for (int ry = RoomId.y; ry < RoomId.y + RoomsCountToDisplay; ry++)
            {
                for (int j = 0; j < RoomSize; j++)
                {
                    for (int k = 0; k < OffsetX; k++)
                        Console.Write(' ');

                    for (int rx = RoomId.x; rx < RoomId.x + RoomsCountToDisplay; rx++)
                    {
                        for (int i = 0; i < RoomSize; i++)
                        {
                            var block = HelpBlock.GetBlockFromId(Rooms[rx, ry][i, j]);
                            Console.ForegroundColor = block.RenderColor;
                            Console.Write(block.RenderChar);
                        }
                    }
                }
            }
        }
    }
}
