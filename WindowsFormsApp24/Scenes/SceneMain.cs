using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp24.Scenes
{
    internal class SceneMain : SceneBase
    {
        public Map Map;

        public SceneMain()
        {
            Map = new Map();
        }

        public override void Update()
        {
            Map.Update();
        }

        public override void Draw()
        {
            Map.Draw();
        }
    }
}
