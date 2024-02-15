using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tooling
{
    public class MouseStates
    {
        public static Dictionary<MouseButtons, bool> ButtonsDown = new Dictionary<MouseButtons, bool>();
        public static Dictionary<MouseButtons, bool> ReleasedButtons = new Dictionary<MouseButtons, bool>();
        public static bool IsDown => ButtonsDown.Count > 0;
        public static bool IsUp => !IsDown;
        public static bool IsButtonDown(MouseButtons button, bool pressed = false) => pressed ? IsButtonPressed(button) : ButtonsDown[button];
        public static bool IsButtonPressed(MouseButtons button) => ButtonsDown[button] && ReleasedButtons[button];
        public static bool PositionChanged => Position != OldPosition;
        public static float LenghtDiff => Position.MinusF(OldPosition).Length();
        public static PointF Position = PointF.Empty, OldPosition = PointF.Empty;
        public static int Delta = 0;
        public static void Initialize()
        {
            ButtonsDown.Clear();
            ReleasedButtons.Clear();
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
            {
                ButtonsDown[b] = false;
                ReleasedButtons[b] = true;
            }
        }
        public static void Update()
        {
            Delta = 0;
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
                ReleasedButtons[b] = !ButtonsDown[b];
        }
    }
}
