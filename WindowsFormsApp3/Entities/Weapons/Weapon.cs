using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities;
using WindowsFormsApp3.SpecialTypes;

namespace WindowsFormsApp3.Entities.Weapons
{
    public abstract class Weapon : Entity
    {
        public DrawableEntity Source = null;
        public Range Level = new Range(1, 5);

        public Weapon(DrawableEntity Source)
        {
            this.Source = Source;
        }
        public abstract void Action();
    }
}
