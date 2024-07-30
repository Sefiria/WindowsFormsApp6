using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion.Triggerables
{
    public class PhysicalObject : Dispf
    {
        public bool Exists = true;
        public virtual void Update() { }
        public PhysicalObject(RoomData_objects po)
        {
        }
        public static PhysicalObject FactoryCreate(RoomData_objects po)
        {
            switch(po.type)
            {
                case Enumerations.PhysicalObjectType.button: return new Button(po);
            }
            return new PhysicalObject(po);
        }
    }
}
