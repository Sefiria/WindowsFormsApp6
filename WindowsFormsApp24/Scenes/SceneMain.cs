using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp24.Scenes
{
    internal class SceneMain : SceneBase
    {
        internal List<Map> Maps = new List<Map>();
        internal int CurrentMapID = -1;
        internal Character MainCharacter;
        internal Cam Cam;

        internal Map Map => Maps[CurrentMapID];

        internal SceneMain()
        {
            Cam = new Cam();
        }

        internal override void Update()
        {
            Map.Update();
            MainCharacter.Update();
            Cam.Update();
        }

        internal override void Draw()
        {
            Map.Draw();
        }
    }
}
