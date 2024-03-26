using _Console.Battle;
using _Console.Core;
using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using static _Console.Core.Skills.Skills;

namespace _Console
{
    partial class Program
    {
        static int Width = 64, Height = 32;
        static int InfosWidth = 20;
        static int DefaultRoomSize = 16;
        static Timer timerUpdate = new Timer() { Enabled = true, Interval = 10 };
        static Timer timerRender = new Timer() { Enabled = true, Interval = 10 };
        static bool Mainloop_Exit = false;
        static Map CurrentMap = null;
        static Vec CurrentRoom = Vec.Empty;
        static Data data = new Data();
        static Vec Look = Vec.Empty;
        static string filepath = $"{Directory.GetCurrentDirectory()}/editedroom.map";
        static bool OnBattle = false;
        static BattleInfo battleInfo = null;
        static object BattleLoopLock = new object();

        static Vec Position = Vec.Empty;
        static char PChar = '•';

        static void Main(string[] args)
        {
            HelpBlock.Initialize();

            Initialize();

            while (!Mainloop_Exit) { lock (BattleLoopLock) { if (OnBattle && battleInfo != null) { BattleConsoleSelection(); } } }
        }
        static void Initialize()
        {
            Load();
            Width = Height = CurrentMap.RoomSize;
            Position = new Vec(Width / 2, Height / 2);

            Console.Title = "";
            Console.SetWindowSize(Width + InfosWidth, Height + 1);
            Console.SetBufferSize(Width + InfosWidth, Height + 1);
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            timerUpdate.Elapsed += UpdateLoop;
            timerRender.Elapsed += RenderLoop;
        }
        static void Dispose()
        {
            timerUpdate.Stop();
            timerRender.Stop();
            timerUpdate.Elapsed -= UpdateLoop;
            timerRender.Elapsed -= RenderLoop;
            Mainloop_Exit = true;
        }
        private static void Load()
        {
            if (!File.Exists(filepath))
                return;

            string content = File.ReadAllText(filepath);

            if (string.IsNullOrWhiteSpace(content))
                return;

            int RoomCount = int.Parse(content.Split('|')[0]);
            content = content.Split('|')[1];

            CurrentMap = new Map(RoomCount, DefaultRoomSize);

            int i = 0;
            for (int rx = 0; rx < RoomCount; rx++)
            {
                for (int ry = 0; ry < RoomCount; ry++)
                {
                    for (int tx = 0; tx < DefaultRoomSize; tx++)
                    {
                        for (int ty = 0; ty < DefaultRoomSize; ty++)
                        {
                            int v = int.Parse(content.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                            CurrentMap[rx, ry][tx, ty] = v;
                            i += 2;
                        }
                    }
                }
            }
        }
        [STAThread]
        static void UpdateLoop(object sender, ElapsedEventArgs e)
        {
            if (OnBattle)
            {
                BattleUpdateLoop();

                if(battleInfo.EndBattle)
                {
                    if (battleInfo.TeamGameOver)
                    {
                        Dispose();
                    }
                    BattleDispose();
                }

                return;
            }

            ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
            ConsoleKey key = KeyInfo.Key;


            if (key == ConsoleKey.Escape)
                Dispose();


            if (key == ConsoleKey.Z)
            {
                Look = new Vec(0, -1);
                if (Position.y == 0)
                {
                    Warp(0, -1);
                }
                else
                if (Position.y > 0 && !Collides(new Vec(0, -1)))
                {
                    Position += Look;
                }
            }

            if (key == ConsoleKey.S)
            {
                Look = new Vec(0, 1);
                if (Position.y == Height - 1)
                {
                    Warp(0, 1);
                }
                else
                if (Position.y < Width - 1 && !Collides(new Vec(0, 1)))
                {
                    Position += Look;
                }
            }

            if (key == ConsoleKey.Q)
            {
                Look = new Vec(-1, 0);
                if (Position.x == 0)
                {
                    Warp(-1, 0);
                }
                else
                if (Position.x > 0 && !Collides(new Vec(-1, 0)))
                {
                    Position += Look;
                }
            }

            if (key == ConsoleKey.D)
            {
                Look = new Vec(1, 0);
                if (Position.x == Width - 1)
                {
                    Warp(1, 0);
                }
                else
                if (Position.x < Height - 1 && !Collides(new Vec(1, 0)))
                {
                    Position += Look;
                }
            }


            if (key == ConsoleKey.Spacebar)
            {
                if (Collides(Look))
                {
                    DoAction(Look);
                }
            }

            if(key == ConsoleKey.A)
            {
                // DEBUG
                Array magicSkills = Enum.GetValues(typeof(MagicalSkills));
                Array specialSkills = Enum.GetValues(typeof(SpecialSkills));
                List<MagicalSkills> magicSkillsList = new List<MagicalSkills>();
                List<SpecialSkills> specialSkillsList = new List<SpecialSkills>();
                foreach (MagicalSkills skill in magicSkills)
                    magicSkillsList.Add(skill);
                foreach (SpecialSkills skill in specialSkills)
                    specialSkillsList.Add(skill);
                data.PlayerTeam[2].magicalSkills.AddRange(magicSkillsList);// toto
                data.PlayerTeam[2].specialSkills.AddRange(specialSkillsList);// toto
                data.PlayerTeam[2].permanentSkills.Add(PermanentSkills.Scan);// toto
                data.AddItemToInventory(new Item("DEBUG", 10));
                battleInfo = new BattleInfo(ref data.PlayerTeam, new[] { new MonsterTest(0), new MonsterTest(1) });
                BattleInitialize();
            }
        }
        private static void Warp(int x, int y)
        {
            if (Position.x < 0 || Position.x >= Width)
                return;

            if (Position.y < 0 || Position.y >= Height)
                return;

            CurrentRoom += new Vec(x, y); // appears in the next room (according to the side where we are)
            Position += new Vec(-x * (Width - 1), -y * (Height - 1));// appears to the opposite side
        }
        private static bool Collides(Vec target)
        {
            if (Position.x + target.x < 0 || Position.x + target.x >= Width)
                return false;

            if (Position.y + target.y < 0 || Position.y + target.y >= Height)
                return false;

            return CurrentMap[CurrentRoom][Position + target] != 0;
        }

        private static void DoAction(Vec target)
        {
            if (Position.x + target.x < 0 || Position.x + target.x >= Width)
                return;

            if (Position.y + target.y < 0 || Position.y + target.y >= Height)
                return;
            
            data.CurrentMap = CurrentMap;
            data.CurrentRoom = CurrentRoom;
            data.TargetTile = Position + target;

            HelpBlock.GetBlockFromId(data.GetTargetTile()).Action(ref data);
        }

        private static void RenderLoop(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (OnBattle)
                {
                    BattleRenderLoop();
                    return;
                }

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(0, 0);
                //Console.Write(CurrentMap[CurrentRoom].GetBuffer(InfosWidth, 0));
                CurrentMap[CurrentRoom].DrawBuffer(InfosWidth, 0);


                int CurItemFlowY = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                foreach (Item item in data.Inventory)
                {
                    if (CurItemFlowY >= Height - 1)
                    {
                        Console.SetCursorPosition(0, Height - 1);
                        Console.Write("...");
                        return;
                    }

                    string sz = $"{item.Name} {HelpBlock.GetBlockFromName(item.Name).RenderChar} : {item.Count}";
                    Console.SetCursorPosition(0, CurItemFlowY++);
                    Console.Write(sz);
                }


                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(InfosWidth + Position.x, Position.y);
                Console.Write(PChar);
            }
            catch(Exception)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Exception");
            }
        }
    }
}
