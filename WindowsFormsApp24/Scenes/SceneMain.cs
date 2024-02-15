using System.Collections.Generic;

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
