using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp12
{
    public class ManageKeyboard
    {
        public static int Cooldown = 0, CooldownMax = 120;
        public delegate void KeyDownHandler(Keys key);
        public static event KeyDownHandler OnKeyDown, OnKeyUp;

        static bool IsDown => Cooldown > 0;
        static readonly int TimeValue = 20;
        static int SpeedMove = 5;
        static int SpeedReSize = 5;
        static Keys CurKeyDown = Keys.None;
        static List<Keys> PlayBot_CurKeys = new List<Keys>();
        static Dictionary<Keys, Action> BindingsWithoutCooldown = new Dictionary<Keys, Action>()
        {
            [Keys.Q] = () => { ManageEntities.MainBoundary.Move(-SpeedMove, 0); },
            [Keys.D] = () => { ManageEntities.MainBoundary.Move(SpeedMove, 0); },
            [Keys.Z] = () => { ManageEntities.MainBoundary.Move(0, -SpeedMove); },
            [Keys.S] = () => { ManageEntities.MainBoundary.Move(0, SpeedMove); },
            [Keys.U] = () => { ManageEntities.MainBoundary.ReSize(-SpeedReSize); },
            [Keys.O] = () => { ManageEntities.MainBoundary.ReSize(SpeedReSize); },
        };
        static Dictionary<Keys, Action> BindingsWithCooldown = new Dictionary<Keys, Action>()
        {
            [Keys.I] = () => { ManageEntities.MainBoundary.Fake(CooldownMax, 0, SpeedMove); },
            [Keys.J] = () => { ManageEntities.MainBoundary.Fake(CooldownMax, 1, SpeedMove); },
            [Keys.K] = () => { ManageEntities.MainBoundary.Fake(CooldownMax, 2, SpeedMove); },
            [Keys.L] = () => { ManageEntities.MainBoundary.Fake(CooldownMax, 3, SpeedMove); },
            [Keys.Space] = () => { ManageEntities.MainBoundary.Hide(60); },
        };

        public static void Reset()
        {
            CurKeyDown = Keys.None;
            PlayBot_CurKeys.Clear();
        }

        public static void Update()
        {
            if (Core.PlayBot == null)
            {
                if (CurKeyDown != Keys.None)
                    ManageKeyDown(CurKeyDown);
            }
            else
            {
                UpdateBot();
            }
        }
        public static void KeyDown(Keys key)
        {
            if (key == CurKeyDown)
                return;

            if (ManageKeyDown(key))
            {
                OnKeyDown?.Invoke(key);
                CurKeyDown = key;
            }
        }
        public static void KeyUp(Keys key)
        {
            if (BindingsWithoutCooldown.ContainsKey(key) || BindingsWithCooldown.ContainsKey(key))
            {
                OnKeyUp?.Invoke(key);
                CurKeyDown = Keys.None;
            }
        }

        private static bool ManageKeyDown(Keys key)
        {
            var result = false;

            if (BindingsWithoutCooldown.ContainsKey(key))
            {
                BindingsWithoutCooldown[key]();
                result = true;
            }

            if (!IsDown)
            {
                if (BindingsWithCooldown.ContainsKey(key))
                {
                    Cooldown = TimeValue * 5;
                    BindingsWithCooldown[key]();
                    result = true;
                }
            }
            else
            {
                Cooldown--;
            }

            return result;
        }


        private static void UpdateBot()
        {
            if (Core.PlayBot.ProgRecords.Count == 0)
                return;

            var e = Core.PlayBot.ProgRecords.First();

            if (Core.MatchTime.ElapsedTicks > e.Time)
            {
                if (e.KeyType == ProgRecord.KeyTypes.Down)
                {
                    SendKeys.Send(Enum.GetName(typeof(Keys), e.Key).ToUpper());
                    if(!PlayBot_CurKeys.Contains(e.Key))
                        PlayBot_CurKeys.Add(e.Key);
                }
                else
                {
                    PlayBot_CurKeys.Remove(e.Key);
                }
                Core.PlayBot.ProgRecords.RemoveAt(0);
            }
            else
            {
                if(PlayBot_CurKeys.Count > 0)
                {
                    string sz;
                    foreach (var key in PlayBot_CurKeys)
                    {
                        sz = Enum.GetName(typeof(Keys), key).ToUpper();
                        switch (sz)
                        {
                            case "SPACE": sz = " "; break;
                        }
                        SendKeys.Send(sz);
                    }
                }
            }
        }
    }
}
