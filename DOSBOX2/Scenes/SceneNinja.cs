using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSBOX2.Common;
using DOSBOX2.GameObjects;

namespace DOSBOX2.Scenes
{
    internal class SceneNinja : IScene
    {
        public BasePlateformCharacter Character;

        public void Init()
        {
            Character = new BasePlateformCharacter(0);
        }

        public void Update()
        {
        }

        public void Dispose()
        {
        }
    }
}
