using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSBOX2.Anims;
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
            new BasePlateformCharacter(1, new AnimController_Player(), new Box(10, 42, 5, 8))
            {
                Name = "Player",
                Inventory = new Inventory(new Inventory(new List<Item>() {
                    new Item((byte)ninja.DB.REF.GUN, 1),
                    new Item((byte)ninja.DB.REF.BULLET, 10)
                }
                ))
            };
            new ColliderEntity() { Name = "Floor", Collider = new Box(0, 50, 150, 50), IsKinetic = true };
            //new ColliderEntity() { Name = "wall", Collider = new Box(50, 30, 70, 50), IsKinetic = true };
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
