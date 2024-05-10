using DOSBOX2.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tooling;

namespace DOSBOX2
{
    public class Core
    {
        public static int CurrentSceneIndex = -1;

        public static List<(string Name, IScene Instance)> Suggestions = new List<(string, IScene)>()
        {
            ("Ninja", new SceneNinja()),
        };

        public static void Update()
        {
            if(CurrentSceneIndex == -1)
            {
                Graphic.DrawRect(0, 5, 5, 8, 8);
                Graphic.FillRect(1, 5, 20, 8, 8);
                Graphic.FillDrawRect(2, 1, 5, 40, 8, 8);
                Text.DisplayText("hello !", 5, 60);
            }
            else
            {
            }
        }
    }
}
