using _Console.Core;
using _Console.Core.Monsters;
using _Console.Core.native;
using _Console.Core.Skills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using static _Console.Core.native.InputsSimulation;
using static _Console.Core.native.NativeGetConsoleSelectionInfo;
using static _Console.Core.Skills.Skills;

namespace _Console
{
    partial class Program
    {
        enum MenuActions
        {
            Attack,
            Defense,
            Magic,
            Special,
            Items,
            Escape
        }

        class Selectable<T>
        {
            public COORD index = new COORD();
            public uint length = 0;
            public string sz = "";
            public T Target;

            public Selectable(short x, short y, T target)
            {
                this.index.X = x;
                this.index.Y = y;
                this.Target = target;
                this.sz = Target == null ? "Cancel" : Target.ToString();
                this.length = (uint)sz.Length;
            }
        };

        static Size BattleWinSize = new Size(96, 32);
        static List<Selectable<MenuActions>> BattleSelectableActions = new List<Selectable<MenuActions>>();
        static int maxOptionWidth = 0;
        static bool MonsterAttackPending = false;

        static void BattleInitialize()
        {
            Console.SetWindowSize(BattleWinSize.Width, BattleWinSize.Height);
            Console.SetBufferSize(BattleWinSize.Width, BattleWinSize.Height);


            BattleSelectableActions.Clear();

            Array actions = Enum.GetValues(typeof(MenuActions));
            short y = 0;
            foreach (MenuActions action in actions)
            {
                Selectable<MenuActions> option = new Selectable<MenuActions>(0, y++, action);
                BattleSelectableActions.Add(option);
                if (option.length > maxOptionWidth)
                    maxOptionWidth = (int)option.length;
            }

            List<string[]> lists = new List<string[]>()
            {
                Enum.GetNames(typeof(MagicalSkills)),
                Enum.GetNames(typeof(SpecialSkills)),
                Enum.GetNames(typeof(PermanentSkills)),
                battleInfo.monsters.Select(x => x.Name).ToArray(),
                battleInfo.team.Select(x => x.Name).ToArray()
            };
            foreach (string[] list in lists)
            {
                foreach (string skill in list)
                {
                    if (skill.Length > maxOptionWidth)
                        maxOptionWidth = skill.Length;
                }
            }

            OnBattle = true;
            BattleRender();
        }
        static void BattleDispose()
        {
            lock (BattleLoopLock)
            {
                Console.SetWindowSize(Width + InfosWidth, Height + 1);
                Console.SetBufferSize(Width + InfosWidth, Height + 1);
                battleInfo = null;
                OnBattle = false;
            }
        }

        static void BattleUpdateLoop()
        {
            if (!battleInfo.IsMonstersTurn)
                return;

            // Monsters Turn

            BattleHandleUselessClick();

            if (MonsterAttackPending)
                return;

            MonsterAttackPending = true;

            Random RND = new Random((int)DateTime.Now.Ticks);
            foreach (Attacker monster in battleInfo.monsters)
            {
                Thread.Sleep(1000);
                int rndAttackType = RND.Next(10);
                if (monster.magicalSkills.Count == 0 || rndAttackType < 8)
                {
                    monster.Attack(battleInfo.team[RND.Next(battleInfo.team.Count)]);
                }
                else
                {
                    monster.Magic(battleInfo.team[RND.Next(battleInfo.team.Count)], monster.magicalSkills[RND.Next(monster.magicalSkills.Count)]);
                }
            }
            Thread.Sleep(1000);

            if (++battleInfo.Turn > battleInfo.monsters.Count)
            {
                battleInfo.Turn = 0;
                battleInfo.ResetAttrMods();
            }

            MonsterAttackPending = false;
        }

        static void BattleConsoleSelection()
        {
            if (!battleInfo.IsTeamTurn)
                return;

            // Team Turn

            if (battleInfo.team[battleInfo.Turn].HP == 0)
            {
                battleInfo.Turn++;
                if (!battleInfo.IsTeamTurn)
                    return;
            }


            Selectable<MenuActions> option = BattleManageClicks(BattleSelectableActions);
            if (option != null)
            {
                if (BattleHandle_Menu(option))
                {
                    battleInfo.Turn++;
                }
                BattleRender_Options(BattleSelectableActions);
            }
        }
        static Selectable<T> BattleManageClicks<T>(List<Selectable<T>> selectables)
        {
            bool MenuHandled = false;
            Selectable<T> select = null;
            CONSOLE_SELECTION_INFO csi;
            GetConsoleSelectionInfo(out csi);
            var (Pointer, Handle) = GetPointerFromObject(csi);
            if (Pointer != IntPtr.Zero)
            {
                if (csi.dwFlags != 0)
                {
                    #region Menu
                    for (int i = 0; i < selectables.Count && !MenuHandled; i++)
                    {
                        select = selectables[i];
                        for (int j = select.index.X; j < select.index.X + select.length && !MenuHandled; j++)
                        {
                            if (new Rectangle(csi.srSelection.Left, csi.srSelection.Top, csi.srSelection.Right, csi.srSelection.Bottom).Contains(BattleMenuRect.X + j + 2, BattleMenuRect.Y + select.index.Y + 1))
                            {
                                MenuHandled = true;
                            }
                        }
                    }
                    #endregion

                    if (!MenuHandled)
                    {
                        InputsSimulation.Send(ScanCodeShort.SPACE);
                    }
                }
            }
            Handle.Free();
            Thread.Sleep(100);

            if (MenuHandled)
            {
                InputsSimulation.Send(ScanCodeShort.SPACE);
                Thread.Sleep(100);
                return select;
            }

            return null;
        }
        static void BattleHandleUselessClick()
        {
            CONSOLE_SELECTION_INFO csi;
            GetConsoleSelectionInfo(out csi);
            var (Pointer, Handle) = GetPointerFromObject(csi);
            if (Pointer != IntPtr.Zero)
            {
                if (csi.dwFlags != 0)
                {
                    Handle.Free();
                    Thread.Sleep(100);
                    InputsSimulation.Send(ScanCodeShort.SPACE);
                    Thread.Sleep(100);
                }
            }
        }
        static bool BattleHandle_Menu(Selectable<MenuActions> option)
        {
            bool handled = true;
            switch ((option as Selectable<MenuActions>).Target)
            {
                default:
                    handled = false;
                    break;

                case MenuActions.Attack:
                    handled = BattleHandle_Attack();
                    break;
                case MenuActions.Defense:
                    battleInfo.GetCurrentTeammate().DEF_ModNextTurn = battleInfo.GetCurrentTeammate().DEF;
                    break;
                case MenuActions.Magic:
                    handled = BattleHandle_Magic();
                    break;
                case MenuActions.Special:
                    handled = BattleHandle_Special();
                    break;
                case MenuActions.Items:
                    handled = BattleHandle_Items();
                    break;
                case MenuActions.Escape:
                    break;
            }

            return handled;
        }

        static Attacker BattleHandle_SelectTarget(bool CanSelectTeammate = true, bool CanSelectMonster = true)
        {
            List<Selectable<Attacker>> BattleSelectableChooseTarget = new List<Selectable<Attacker>>();
            short y = 0;
            BattleSelectableChooseTarget.Add(new Selectable<Attacker>(0, y++, null));// Cancel
            if(CanSelectMonster)
                foreach (Attacker monster in battleInfo.monsters)
                    BattleSelectableChooseTarget.Add(new Selectable<Attacker>(0, y++, monster));
            if(CanSelectTeammate)
                foreach (Attacker teammate in battleInfo.team)
                    BattleSelectableChooseTarget.Add(new Selectable<Attacker>(0, y++, teammate));
            BattleRender_Options(BattleSelectableChooseTarget);
            Selectable<Attacker> optionTarget = null;
            do
            {
                optionTarget = BattleManageClicks(BattleSelectableChooseTarget);
            }
            while (optionTarget == null);

            Attacker target = (optionTarget as Selectable<Attacker>).Target;

            return target;
        }
        static bool BattleHandle_Option<T>(List<T> options, Action<Attacker, Attacker, T> action) where T : struct
        {
            List<Selectable<T?>> BattleSelectableOptions = new List<Selectable<T?>>();
            short y = 0;
            BattleSelectableOptions.Add(new Selectable<T?>(0, y++, null));// Cancel
            foreach (T option in options)
            {
                BattleSelectableOptions.Add(new Selectable<T?>(0, y++, option));
            }
            ChooseOption:
            BattleRender_Options(BattleSelectableOptions);
            Selectable<T?> selectedSelectableOption = null;
            do
            {
                selectedSelectableOption = BattleManageClicks(BattleSelectableOptions);
            }
            while (selectedSelectableOption == null);

            T? selectedOption = selectedSelectableOption.Target;
            if (selectedOption == null)
            {
                return false; // Cancel
            }


            Attacker target = BattleHandle_SelectTarget();
            if (target == null)
            {
                goto ChooseOption; // Cancel
            }

            action(target, battleInfo.GetCurrentTeammate(), selectedOption.Value);

            return true;
        }

        static bool BattleHandle_Attack()
        {
            Attacker target = BattleHandle_SelectTarget(false, true);

            if (target == null)
            {
                return false; // Cancel
            }

            if(battleInfo.GetCurrentTeammate().Attack(target))
            {
                battleInfo.DestroyTarget(target, BattleRenderMonters);
            }

            return true;
        }
        static bool BattleHandle_Magic()
        {
            return BattleHandle_Option(
                    battleInfo.GetCurrentTeammate().magicalSkills,
                    (target, teammate, magic) => { if (teammate.Magic(target, magic)) battleInfo.DestroyTarget(target, BattleRenderMonters); }
                );
        }
        static bool BattleHandle_Special()
        {
            return BattleHandle_Option(
                    battleInfo.GetCurrentTeammate().specialSkills,
                    (target, teammate, special) => { if (teammate.Special(target, special)) battleInfo.DestroyTarget(target, BattleRenderMonters); }
                );
        }
        static bool BattleHandle_Items()
        {
            List<NNItem> items = new List<NNItem>();
            foreach (Item item in data.Inventory)
            {
                if(typeof(ItemEffects).GetMethod(item.Name) == null)
                    continue;

                items.Add(new NNItem(item));
            }

            return BattleHandle_Option(
                    items,
                    (target, teammate, item) => { data.RemoveItemToInventory(item); if(teammate.UseItem(target, item)) battleInfo.DestroyTarget(target, BattleRenderMonters); }
                );
        }
    }
}
