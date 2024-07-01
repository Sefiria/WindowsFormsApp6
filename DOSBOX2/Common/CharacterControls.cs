using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Tooling;

namespace DOSBOX2.Common
{
    internal class CharacterControls
    {
        public enum KeyBindings
        {
            Left, Right, Jump, Crouch, Shot, Action, LookUp
        }

        public static Dictionary<KeyBindings, KB.Key> DefaultKeyBinds => new Dictionary<KeyBindings, KB.Key>()
        {
            [KeyBindings.Left] = KB.Key.Q,
            [KeyBindings.Right] = KB.Key.D,
            [KeyBindings.Jump] = KB.Key.Space,
            [KeyBindings.Crouch] = KB.Key.LeftCtrl,
            [KeyBindings.Shot] = KB.Key.Numpad0,
            [KeyBindings.Action] = KB.Key.LeftAlt,
            [KeyBindings.LookUp] = KB.Key.Up,
        };
        public Dictionary<KeyBindings, KB.Key> KeyBinds = new Dictionary<KeyBindings, KB.Key>(DefaultKeyBinds);
        public Dictionary<KeyBindings, bool> ForcedStates = new Dictionary<KeyBindings, bool>();

        public bool IsMoving => ForcedStates[KeyBindings.Left] || ForcedStates[KeyBindings.Right] || KB.IsKeyDown(KeyBinds[KeyBindings.Left]) || KB.IsKeyDown(KeyBinds[KeyBindings.Right]);

        public CharacterControls()
        {
            KeyBinds.Keys.ToList().ForEach(x => ForcedStates[x] = false);
        }

        public void Update()
        {
            var keys = new Dictionary<KeyBindings, bool>(ForcedStates).Keys;
            foreach (var key in keys)
                ForcedStates[key] = false;
        }

        public void ForceState(KeyBindings key, bool state) => ForcedStates[key] = state;
    }
}
