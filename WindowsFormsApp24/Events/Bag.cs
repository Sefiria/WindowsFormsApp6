using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using Tooling.UI;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Bag : Event
    {
        internal Bag(int x, int y, int z) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y, z)
        {
        }
        internal Bag(float x, float y, float z) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y, z)
        {
        }
        internal Bag(NamedObjects obj, long grow_ticks, int count, int x, int y, int z) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y)
        {
            Initialize(obj, grow_ticks, count);
        }
        internal Bag(NamedObjects obj, long grow_ticks, int count, float x, float y, float z) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y)
        {
            Initialize(obj, grow_ticks, count);
        }

        private void Initialize(NamedObjects obj, long grow_ticks, int count)
        {
            var nm = Enum.GetName(typeof(NamedObjects), obj);
            Name = $"Bag of {nm} seeds";
            Data["Name"] = $"{nm}-seed";
            Data["Image"] = Core.NamedTextures[obj];
            Data["SeedCount"] = count;
            Data["grow_ticks"] = grow_ticks;
            OnPrimaryAction += () =>
            {
                if ((int)Data["SeedCount"] > 0)
                {
                    PredefinedActions[PredefinedAction.Loot](this);
                    Data["SeedCount"] = (int)Data["SeedCount"] - 1;
                }
            };
            OnSecondaryAction += () =>
            {
                PredefinedActions[PredefinedAction.TakeDrop](this);
            };
        }

        internal override void Draw()
        {
            base.Draw();
            if (MouseHover)
                DrawExtraInfos();
        }
        internal override void DrawExtraInfos()
        {
            var cam = Core.Cam;
            UIDisplay.Display(Core.Instance.gUI, $"{Data["Name"]} x {Data["SeedCount"]}", X - cam.X, Y - cam.Y - Core.TileSize);
        }
    }
}
