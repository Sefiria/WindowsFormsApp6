using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp21Isometric
{
    public class GameManager
    {
        private readonly Map _map = new Map();

        public void Update()
        {
            InputManager.Update();
            _map.Update();
        }

        public void Draw()
        {
            _map.Draw();
        }
    }
}
