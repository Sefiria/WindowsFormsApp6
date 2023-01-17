using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsFormsApp12
{
    public class Data
    {
        public static bool Ended = true;
        public static Dictionary<Key, bool> Released = new Dictionary<Key, bool>()
        {
            [Key.A] = true,
        };

        public static void End()
        {
            if (Core.RecordBot != null)
            {
                Core.RecordBot.RecStop();
                Prog.Save(Core.RecordBot);
                Core.RecordBot = null;
            }

            Ended = true;
            Core.MatchTime.Stop();
        }

        public static void EndProcessUpdate()
        {
            if (Keyboard.IsKeyDown(Key.Space))
            {
                ManageEntities.Reset();
                ManageKeyboard.Reset();
                ManageMouse.Restart();
                Ended = false;
                Core.MatchTime.Restart();
                if(Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Core.RecordBot = new Prog();
                    Core.RecordBot.RecStart();
                }
                else if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    Core.PlayBot = Prog.Load(0);
                }
            }

            if (Keyboard.IsKeyDown(Key.A) && Released[Key.A])
            {
                ManageMouse.DisplayUIMode++;
                if (ManageMouse.DisplayUIMode > 6)
                    ManageMouse.DisplayUIMode = 0;
            }

            var released = new Dictionary<Key, bool>(Released);
            foreach (var pair in released)
                Released[pair.Key] = Keyboard.IsKeyUp(pair.Key);
        }
        public static void EndProcessDraw()
        {
        }
    }
}
