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
        public PhysicalObject()
        {
        }
        public static PhysicalObject FactoryCreate(byte room_id, RoomData_objects po)
        {
            switch(po.type)
            {
                case Enumerations.PhysicalObjectType.button: return new Button(room_id, po);
            }
            return new PhysicalObject();
        }
    }
}
