using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp21Isometric
{
    public static class InputManager
    {
        private static Point _direction;
        public static Point Direction => _direction;
        public static Point MousePosition => Mouse.GetState().Position;

        public static void Update()
        {
            _direction = Point.Empty;

            if (Keyboard.IsKeyToggled(Key.W)) _direction.Y--;
            if (Keyboard.IsKeyToggled(Key.S)) _direction.Y++;
            if (Keyboard.IsKeyToggled(Key.A)) _direction.X--;
            if (Keyboard.IsKeyToggled(Key.D)) _direction.X++;
        }
    }
}
