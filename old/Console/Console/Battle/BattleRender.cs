using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _Console
{
    partial class Program
    {
        static object RenderLock = new object();

        private class BattleTeamInfoXLocations
        {
            /*
                Name****  HP 9999/9999  MP 999/999  SP 999/999  DEF 99  STR 99  INT 99
                ..........000000000000000011111111111111112222222222222222333333333333
                ..........0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789AB

                OFFSET  0A

                ATTR	LOC	LGTH
                HP	    03	4
                MaxHP	08	4
                MP	    11	3
                MaxMP	15	3
                SP	    1D	3
                MaxSP	21	3
                DEF	    2A	2
                STR	    32	2
                INT	    3A	2
            */
            public static int Name => 0;

            public static int _OFFSET => 0x0A;
            public static int HP => _OFFSET + 0x03;
            public static int MaxHP => _OFFSET + 0x08;
            public static int MP => _OFFSET + 0x11;
            public static int MaxMP => _OFFSET + 0x15;
            public static int SP => _OFFSET + 0x1D;
            public static int MaxSP => _OFFSET + 0x21;
            public static int DEF => _OFFSET + 0x2A;
            public static int STR => _OFFSET + 0x32;
            public static int INT => _OFFSET + 0x3A;
        }

        static Rectangle BattleMenuRect => new Rectangle(0, BattleWinSize.Height - 16, BattleWinSize.Width - 1, 16);
        static bool RenderingAttr = false;
        
        static void BattleRender()
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            BattleRenderMonters();
            BattleRenderMenu();
        }

        static void BattleRenderMonters()
        {
            lock (RenderLock)
            {
                Console.SetCursorPosition(0, 0);
                for (int y = 0; y < BattleMenuRect.Y; y++)
                    Console.Write("                                                                                                ");
                

                foreach (Monster monster in battleInfo.monsters)
                {
                    string[] monsterPaintLines = monster.Paint.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < monsterPaintLines.Length; i++)
                    {
                        Console.SetCursorPosition(monster.RenderPosX, 2 + i);
                        Console.Write(monsterPaintLines[i]);
                    }
                }
            }
        }
        static void BattleRenderMonsters_Attributes()
        {
            int scanLevel = 0;// scanLevel € [1;3]
            #region GetMaxScanOfTeam
            battleInfo.team.ForEach(x => x.permanentSkills.ForEach(i => { if ((int)i < 4 && (int)i > scanLevel) scanLevel = (int)i; }));
            foreach(Attacker attacker in battleInfo.team)
            {
                foreach(int skill in attacker.permanentSkills.Cast<int>())
                {
                    if (skill < 4 && skill > scanLevel)
                        scanLevel = skill;

                    if (scanLevel == 3)
                        break;
                }

                if (scanLevel == 3)
                    break;
            }
            #endregion

            var monsters = battleInfo.monsters;
            foreach (Monster monster in monsters)
            {
                // Show monsters HP
                if (scanLevel > 0)
                {
                    Console.SetCursorPosition(monster.RenderPosX, 2 + monster.Paint.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length + 1);
                    Console.Write($"{monster.HP}/{monster.MaxHP}");
                }
                // Show in more monsters DEF, STR
                if (scanLevel > 1)
                {
                    // TODO
                }
                // Show in more monsters MP, INT, and ElementWeakness
                if (scanLevel > 2)
                {
                    // TODO
                }
            }
        }

        static void BattleRenderMenu()
        {
            BattleRender_OptionsBorders();
            BattleRender_Options(BattleSelectableActions);
            BattleRender_TeamInfo();
        }
        static void BattleRender_OptionsBorders()
        {
            for (int x = 0; x < BattleMenuRect.Width; x++)
            {
                Console.SetCursorPosition(BattleMenuRect.X + x, BattleMenuRect.Y);
                Console.Write('═');
                Console.SetCursorPosition(BattleMenuRect.X + x, BattleMenuRect.Y + BattleMenuRect.Height - 1);
                Console.Write('═');
            }
            for (int y = 0; y < BattleMenuRect.Height; y++)
            {
                Console.SetCursorPosition(BattleMenuRect.X, BattleMenuRect.Y + y);
                Console.Write('║');
                Console.SetCursorPosition(BattleMenuRect.X + BattleMenuRect.Width - 1, BattleMenuRect.Y + y);
                Console.Write('║');
                if (y > 0 && y < BattleMenuRect.Height - 1)
                {
                    Console.SetCursorPosition(BattleMenuRect.X + maxOptionWidth + 4, BattleMenuRect.Y + y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write('│');
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.SetCursorPosition(BattleMenuRect.X, BattleMenuRect.Y);
            Console.Write('╔');
            Console.SetCursorPosition(BattleMenuRect.X + BattleMenuRect.Width - 1, BattleMenuRect.Y);
            Console.Write('╗');
            Console.SetCursorPosition(BattleMenuRect.X, BattleMenuRect.Y + BattleMenuRect.Height - 1);
            Console.Write('╚');
            Console.SetCursorPosition(BattleMenuRect.X + BattleMenuRect.Width - 1, BattleMenuRect.Y + BattleMenuRect.Height - 1);
            Console.Write('╝');
        }
        static void BattleRender_ClearOptions()
        {
            string Xbuf = "";
            for (int x = 0; x < maxOptionWidth; x++)
                Xbuf += ' ';
            for (int y = 1; y < BattleMenuRect.Height - 1; y++)
            {
                Console.SetCursorPosition(BattleMenuRect.X + 2, BattleMenuRect.Y + y);
                Console.Write(Xbuf);
            }
        }
        static void BattleRender_Options<T>(List<Selectable<T>> Selectables)
        {
            lock (RenderLock)
            {

                BattleRender_ClearOptions();

                foreach (Selectable<T> option in Selectables)
                {
                    Console.SetCursorPosition(BattleMenuRect.X + option.index.X + 2, BattleMenuRect.Y + option.index.Y + 1);
                    Console.Write(option.sz);
                }

            }
        }
        static void BattleRender_TeamInfo()
        {
            int y = 1;
            foreach(Attacker player in battleInfo.team)
            {
                #region Labels
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.Name, BattleMenuRect.Y + y);
                Console.Write(player.Name);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations._OFFSET, BattleMenuRect.Y + y);
                Console.Write($"HP     /      MP    /     SP    /     DEF     STR     INT   ");
                #endregion

                #region Attributes
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.Name, BattleMenuRect.Y + y);
                if (battleInfo.Turn < battleInfo.team.Count && battleInfo.Turn == battleInfo.team.IndexOf(player))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(player.Name);
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.HP, BattleMenuRect.Y + y);
                if(player.HP < player.MaxHP * 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(player.HP);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxHP, BattleMenuRect.Y + y);
                Console.Write(player.MaxHP);

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MP, BattleMenuRect.Y + y);
                if (player.MP < player.MaxMP * 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(player.MP);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxMP, BattleMenuRect.Y + y);
                Console.Write(player.MaxMP);

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.SP, BattleMenuRect.Y + y);
                Console.Write(player.SP);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxSP, BattleMenuRect.Y + y);
                Console.Write(player.MaxSP);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.DEF, BattleMenuRect.Y + y);
                Console.Write(player.DEF);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.STR, BattleMenuRect.Y + y);
                Console.Write(player.STR);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.INT, BattleMenuRect.Y + y);
                Console.Write(player.INT);
                #endregion

                y++;
            }
        }
        static void BattleRender_TeamInfo_Attributes()
        {
            if (RenderingAttr || !OnBattle)
                return;
            
            RenderingAttr = true;

            int y = 1;
            var team = battleInfo.team;
            foreach (Attacker player in team)
            {
                if (!player.AnyAttrChanged)
                {
                    y++;
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.Name, BattleMenuRect.Y + y);
                if (battleInfo.Turn < battleInfo.team.Count && battleInfo.Turn == battleInfo.team.IndexOf(player))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(player.Name);
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.HP, BattleMenuRect.Y + y);
                Console.Write("    ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.HP, BattleMenuRect.Y + y);
                if (player.HP < player.MaxHP * 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(player.HP);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxHP, BattleMenuRect.Y + y);
                Console.Write(player.MaxHP);

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MP, BattleMenuRect.Y + y);
                Console.Write("   ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MP, BattleMenuRect.Y + y);
                if (player.MP < player.MaxMP * 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(player.MP);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxMP, BattleMenuRect.Y + y);
                Console.Write(player.MaxMP);

                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.SP, BattleMenuRect.Y + y);
                Console.Write("   ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.SP, BattleMenuRect.Y + y);
                Console.Write(player.SP);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.MaxSP, BattleMenuRect.Y + y);
                Console.Write(player.MaxSP);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.DEF, BattleMenuRect.Y + y);
                Console.Write("  ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.DEF, BattleMenuRect.Y + y);
                Console.Write(player.DEF);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.STR, BattleMenuRect.Y + y);
                Console.Write("  ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.STR, BattleMenuRect.Y + y);
                Console.Write(player.STR);
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.INT, BattleMenuRect.Y + y);
                Console.Write("  ");
                Console.SetCursorPosition(BattleMenuRect.X + 2 + maxOptionWidth + 4 + BattleTeamInfoXLocations.INT, BattleMenuRect.Y + y);
                Console.Write(player.INT);

                y++;
            }

            RenderingAttr = false;
        }

        static void BattleRenderLoop()
        {
            lock (RenderLock)
            {
                if (!OnBattle)
                    return;
                BattleRender_TeamInfo_Attributes();
                BattleRenderMonsters_Attributes();
            }
        }
    }
}
