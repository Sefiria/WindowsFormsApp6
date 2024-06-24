using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSBOX2.Common;
using DOSBOX2.GameObjects;
using Tooling;

namespace DOSBOX2.Scenes
{
    internal class SceneNinja : IScene
    {
        EntityManager EntityManager;

        public SceneNinja()
        {
            EntityManager = new EntityManager();
        }

        public void Init()
        {
            Entity.Manager = EntityManager;
            var player = new BasePlateformCharacter(1) { Name = "Player", Collider = new Box(0, 0, 5, 8) };
            var floor = new Entity() { Name = "Floor", Collider = new Box(0, 50, 100, 50) };

            player.x = 10;
            player.y = floor.y - player.h;
            player.Texture = GraphicExt.CreateBatch(@"
33033
33033
30003
03030
03030
33033
30303
03330
");
            player.Inventory.Add(ninja.DB.REF.GUN, 1);
            player.Inventory.Add(ninja.DB.REF.BULLET, 10);
            player.Draw(true, false);
            floor.Draw(true, false);
        }

        public void Update()
        {
            // BeforeUpdate
            EntityManager.BeforeUpdate();

            // Update
            EntityManager.Update();

            // AfterUpdate
            EntityManager.AfterUpdate();

            //// Draw
            EntityManager.Draw();
        }

        public void Dispose()
        {
        }
    }
}
