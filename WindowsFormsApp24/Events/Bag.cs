using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Bag : Event
    {
        internal Bag(float x, float y) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y)
        {
        }
        internal Bag(NamedObjects obj, long grow_ticks, int count, float x, float y) : base(Core.NamedTextures[NamedObjects.Bag], true, x, y)
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
    }
}
