using _Console.Core.native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using static _Console.Core.native.NativeGetConsoleSelectionInfo;
using System.Threading;
using static _Console.Core.native.InputsSimulation;
using System.IO;
using _Console.Core;

namespace Editor
{
    class Program
    {
        class Selectable
        {
            public COORD index = new COORD();
            public uint length = 0;
            public string sz = "";
            public int associatedTile = 0;

            public Selectable(short x, short y, string sz, int associatedTile)
            {
                this.index.X = x;
                this.index.Y = y;
                this.length = (uint)sz.Length;
                this.sz = sz;
                this.associatedTile = associatedTile;
            }
        };

        static bool Mainloop_Exit = false;
        static int InfosWidth = 16;
        static Map map = null;
        static int RoomCount;// on double array, e.g for RoomCount = 6 => there is 6 * 6 rooms
        static int DefaultRoomSize = 16;
        static Vec TopLeftRoomToDisplay = Vec.Empty;
        static List<Selectable> Selectables;
        static Random rnd = new Random();
        static Selectable SelectedTile = null;
        static string filepath = $"{Directory.GetCurrentDirectory()}/editedroom.map";
        static List<VirtualKeyShort> KeysReleased = new List<VirtualKeyShort>();
        static List<VirtualKeyShort> ManagedKeys = new List<VirtualKeyShort>()
        {
                VirtualKeyShort.KEY_S,
                VirtualKeyShort.UP,
                VirtualKeyShort.DOWN,
                VirtualKeyShort.LEFT,
                VirtualKeyShort.RIGHT
        };

        static void Main(string[] args)
        {
            HelpBlock.Initialize();

            bool initMap = !Load();

            if (initMap)
            {
                Console.Write("> Rooms Count (linear) ? ");
                int charRead = 0;
                int intResult = 0;
                while (!int.TryParse("" + (char)charRead, out intResult) || intResult <= 0)
                {
                    charRead = Console.Read();
                }
                RoomCount = intResult;

                Console.Clear();
            }

            Initialize(initMap);

            RenderLoop();
            Task.Run((Action)UpdateLoop);

            while (!Mainloop_Exit) { ConsoleSelection(); }
        }
        static void Dispose()
        {
            Mainloop_Exit = true;
        }
        static void Initialize(bool initMap = true)
        {
            Console.Title = "";
            Console.SetWindowSize(DefaultRoomSize * 3 + InfosWidth, DefaultRoomSize * 3 + 1);
            Console.SetBufferSize(DefaultRoomSize * 3 + InfosWidth, DefaultRoomSize * 3 + 1);
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;

            if (initMap)
            {
                map = new Map(RoomCount, DefaultRoomSize);
                for (int rx = 0; rx < RoomCount; rx++)
                {
                    for (int ry = 0; ry < RoomCount; ry++)
                    {
                        for (int i = 0; i < DefaultRoomSize; i++)
                        {
                            map[rx, ry][i, 0] = 1;
                            map[rx, ry][i, DefaultRoomSize - 1] = 1;
                        }
                        for (int j = 0; j < DefaultRoomSize; j++)
                        {
                            map[rx, ry][0, j] = 1;
                            map[rx, ry][DefaultRoomSize - 1, j] = 1;
                        }
                    }
                }
            }
            Selectables = new List<Selectable>();
            int BlocksCount = Enum.GetValues(typeof(HelpBlock.Blocks)).Length;
            for (int i = -1; i < BlocksCount; i++)
            {
                Console.SetCursorPosition(1, 1 + (i + 1));
                string sz;
                if (i == -1)
                    sz = "TileInfo:?";
                else
                    sz = $"{Enum.GetName(typeof(HelpBlock.Blocks), i)}:{HelpBlock.GetBlockFromId(i).RenderChar}";
                Selectable select = new Selectable(1, (short)(i == -1 ? 2 : i + 3), sz, i);
                Selectables.Add(select);
                if (i == 0)
                {
                    SelectedTile = select;
                }
            }
        }
        private static void ConsoleSelection()
        {
            bool MenuHandled = false, EditAreaHandled = false;
            Selectable select = null;
            CONSOLE_SELECTION_INFO csi;
            GetConsoleSelectionInfo(out csi);
            var (Pointer, Handle) = GetPointerFromObject(csi);
            if (Pointer != IntPtr.Zero)
            {
                if (csi.dwFlags != 0)
                {
                    #region Menu
                    for (int i = 0; i < Selectables.Count && !MenuHandled; i++)
                    {
                        select = Selectables[i];
                        for (int j = select.index.X; j < select.index.X + select.length && !MenuHandled; j++)
                        {
                            if (new Rectangle(csi.srSelection.Left, csi.srSelection.Top, csi.srSelection.Right, csi.srSelection.Bottom).Contains(j, select.index.Y))
                            {
                                MenuHandled = true;
                            }
                        }
                    }
                    #endregion

                    #region EditArea
                    if (new Rectangle(InfosWidth, 0, InfosWidth + DefaultRoomSize * 3, DefaultRoomSize * 3).Contains(csi.srSelection.Left, csi.srSelection.Top))
                    {
                        EditAreaHandled = true;
                    }
                    #endregion

                    if (!MenuHandled && !EditAreaHandled)
                    {
                        InputsSimulation.Send(ScanCodeShort.SPACE);
                    }
                }
            }
            Handle.Free();
            Thread.Sleep(120);
            if (MenuHandled || EditAreaHandled)
            {
                InputsSimulation.Send(ScanCodeShort.SPACE);

                // Remove any TileInfo
                Console.BackgroundColor = ConsoleColor.Black;
                for (int i = 0; i < InfosWidth - 1; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write(' ');
                }

                if (MenuHandled)
                {
                    SelectedTile = select;
                    ResetBGMenuTiles();
                    SelectBGMenuTile();
                }
                if (EditAreaHandled)
                {
                    char tilechar = SelectedTile.sz[SelectedTile.sz.Length - 1];
                    int rx = 0;
                    int ry = 0;
                    int tx = csi.srSelection.Left - InfosWidth;
                    int ty = csi.srSelection.Top;
                    while (tx >= DefaultRoomSize)
                    {
                        tx -= DefaultRoomSize;
                        rx++;
                    }
                    while (ty >= DefaultRoomSize)
                    {
                        ty -= DefaultRoomSize;
                        ry++;
                    }

                    if (SelectedTile.associatedTile == -1)// TileInfo
                    {
                        Console.SetCursorPosition(0, 0);
                        var block = HelpBlock.GetBlockFromId(map[rx + TopLeftRoomToDisplay.x, ry + TopLeftRoomToDisplay.y][tx, ty]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = block.RenderColor;
                        Console.Write(block.EnumValue.ToString());
                    }
                    else
                    {
                        map[rx + TopLeftRoomToDisplay.x, ry + TopLeftRoomToDisplay.y][tx, ty] = SelectedTile.associatedTile;
                        Console.SetCursorPosition(csi.srSelection.Left, csi.srSelection.Top);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(HelpBlock.GetBlockFromId(SelectedTile.associatedTile).RenderChar);
                    }
                }
            }
        }
        private static void ResetBGMenuTiles()
        {
            foreach (Selectable select in Selectables)
            {
                Console.SetCursorPosition(select.index.X, select.index.Y);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(select.sz);
                Console.ResetColor();
            }
        }
        private static void SelectBGMenuTile()
        {
            Console.SetCursorPosition(SelectedTile.index.X, SelectedTile.index.Y);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(SelectedTile.sz);
            Console.ResetColor();
        }

        static void ManageKeys()
        {
            foreach (VirtualKeyShort key in ManagedKeys)
            {
                if (KeysReleased.Contains(key) && (GetKeyState(key) & KEY_PRESSED) != 0) KeysReleased.Remove(key);
                else if (!KeysReleased.Contains(key)) KeysReleased.Add(key);
            }
        }
        static bool KeyPressed(VirtualKeyShort key) => KeysReleased.Contains(key) && (GetKeyState(key) & KEY_PRESSED) != 0;
        static void UpdateLoop()
        {
            while (!Mainloop_Exit)
            {
                if (KeyPressed(VirtualKeyShort.KEY_S))
                    Save();

                Vec prevTopLeftRoomToDisplay = new Vec(TopLeftRoomToDisplay);
                if (KeyPressed(VirtualKeyShort.UP) && TopLeftRoomToDisplay.y > 0)
                    TopLeftRoomToDisplay.y--;
                if (KeyPressed(VirtualKeyShort.DOWN) && TopLeftRoomToDisplay.y < map.RoomsCount - 3)
                    TopLeftRoomToDisplay.y++;
                if (KeyPressed(VirtualKeyShort.LEFT) && TopLeftRoomToDisplay.x > 0)
                    TopLeftRoomToDisplay.x--;
                if (KeyPressed(VirtualKeyShort.RIGHT) && TopLeftRoomToDisplay.x < map.RoomsCount - 3)
                    TopLeftRoomToDisplay.x++;
                if (TopLeftRoomToDisplay != prevTopLeftRoomToDisplay)
                    RenderLoop();

                ManageKeys();

                Thread.Sleep(50);
            }
        }

        private static bool Load()
        {
            if (!File.Exists(filepath))
                return false;

            string content = File.ReadAllText(filepath);

            if (string.IsNullOrWhiteSpace(content))
                return false;

            RoomCount = int.Parse(content.Split('|')[0]);
            content = content.Split('|')[1];

            map = new Map(RoomCount, DefaultRoomSize);

            int k = 0;
            for (int rx = 0; rx < RoomCount; rx++)
            {
                for (int ry = 0; ry < RoomCount; ry++)
                {
                    for (int tx = 0; tx < DefaultRoomSize; tx++)
                    {
                        for (int ty = 0; ty < DefaultRoomSize; ty++)
                        {
                            int v = int.Parse(content.Substring(k, 2), System.Globalization.NumberStyles.HexNumber);
                            map[rx, ry][tx, ty] = v;
                            k += 2;
                        }
                    }
                }
            }

            return true;
        }
        private static void Save()
        {
            string content = "";

            content += $"{RoomCount}|";

            for (int rx = 0; rx < RoomCount; rx++)
            {
                for (int ry = 0; ry < RoomCount; ry++)
                {
                    for (int tx = 0; tx < DefaultRoomSize; tx++)
                    {
                        for (int ty = 0; ty < DefaultRoomSize; ty++)
                        {
                            content += map[rx, ry][tx, ty].ToString("X2");
                        }
                    }
                }
            }

            File.WriteAllText(filepath, content);
        }

        private static void RenderLoop()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            //Console.Write(map.GetBuffer(3, TopLeftRoomToDisplay, InfosWidth, 0));
            map.DrawBuffer(3, TopLeftRoomToDisplay, InfosWidth, 0);

            for (int j = 0; j < DefaultRoomSize * 3; j++)
            {
                Console.SetCursorPosition(InfosWidth - 1, j);
                Console.Write('│');
            }

            DisplayTopLeftRoom();
            
            foreach(Selectable select in Selectables)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(select.index.X, select.index.Y);
                Console.Write(select.sz);
            }
        }

        private static void DisplayTopLeftRoom()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            Console.Write(TopLeftRoomToDisplay);
        }

        static void OnClick(Func<bool> Method, Action<CONSOLE_SELECTION_INFO> DrawMethod)
        {
            bool handled = false;
            CONSOLE_SELECTION_INFO csi;
            GetConsoleSelectionInfo(out csi);
            var (Pointer, Handle) = GetPointerFromObject(csi);
            if (Pointer != IntPtr.Zero)
            {
                if (csi.dwFlags != 0)
                {
                    handled = Method();

                    if (!handled)
                    {
                        InputsSimulation.Send(ScanCodeShort.SPACE);
                    }
                }
            }
            Handle.Free();
            Thread.Sleep(100);
            if (handled)
            {
                InputsSimulation.Send(ScanCodeShort.SPACE);
                DrawMethod(csi);
            }
        }
    }
}
