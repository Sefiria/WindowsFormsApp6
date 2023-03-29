using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp15.items;
using WindowsFormsApp15.structure;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15
{
    internal class UserUI
    {
        Structure InHand = null;
        sbyte Rotation = 0;
        List<Structure> PossibleCrafts = null;
        sbyte TimeToDestroy = sbyte.MaxValue;

        int hand_selection = 0;

        public UserUI()
        {
            PossibleCrafts = new List<Structure>()
            {
                new StructureConveyor(),
                new StructureDrill(),
                new StructureFurnace(),
            };

            InHand = (Structure)Activator.CreateInstance(PossibleCrafts[hand_selection].GetType());
        }

        public void MouseWheel(MouseEventArgs e)
        {
            hand_selection += (e.Delta > 0 ? 1 : -1);
            if (hand_selection >= PossibleCrafts.Count) hand_selection = 0;
            if (hand_selection < 0) hand_selection = PossibleCrafts.Count - 1;
            InHand = (Structure)Activator.CreateInstance(PossibleCrafts[hand_selection].GetType());
        }

        public void MouseInput(MouseEventArgs e, bool up = false)
        {
            if(up)
            {
                TimeToDestroy = sbyte.MaxValue;
                return;
            }

            DestroyStructure(e);
            AddStructure(e);
        }
        private void DestroyStructure(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Data.Instance.ThereStructureAt(Core.MouseSnap))
            {
                if (TimeToDestroy <= 0)
                {
                    Data.Instance.Structures.Remove(Core.MouseSnap);
                    TimeToDestroy = sbyte.MaxValue;
                }
                else
                {
                    var s = Data.Instance.GetStructureAt(Core.MouseSnap);
                    TimeToDestroy -= (sbyte)(s is StructureConveyor ? 10 : (s is IMoveInfos ? 2 : 1));
                }
            }
            else
                TimeToDestroy = sbyte.MaxValue;
        }
        private void AddStructure(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (!Data.Instance.ThereStructureAt(Core.MouseSnap) || Data.Instance.GetStructureAt(Core.MouseSnap) is StructureConveyor))
            {
                object structure;
                var rotationStructures = new List<Type> {
                    typeof(StructureConveyor),
                    typeof(StructureDrill),
                    typeof(StructureFurnace) };
                if (rotationStructures.Contains(PossibleCrafts[hand_selection].GetType()))
                    structure = Activator.CreateInstance(PossibleCrafts[hand_selection].GetType(), new object[] { Core.MouseSnap, Rotation });
                else
                    structure = Activator.CreateInstance(PossibleCrafts[hand_selection].GetType(), new object[] { Core.MouseSnap });
                if (Data.Instance.ThereStructureAt(Core.MouseSnap))
                    Data.Instance.Structures.Remove(Core.MouseSnap);
                Data.Instance.Structures.Add(Core.MouseSnap, (Structure)structure);
            }
        }
        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.R))
            {
                Rotation += (sbyte)(KB.LeftShift ? -1 : 1);
                sbyte len = (sbyte)(Enum.GetNames(typeof(Way)).Length - 1);
                if (Rotation < 0) Rotation = len;
                if (Rotation > len) Rotation = 0;
                if (InHand is StructureConveyor)
                    (InHand as StructureConveyor).Way = (Way)Rotation;
            }
        }

        public void Display()
        {
            InHand.vec = Core.MouseVec;
            InHand.Display();
            InHand.FrontDisplay();

            Core.g.DrawImage(arrows[(Way)Rotation], Core.MouseVec.snap(Core.TSZ).pt);

            if (TimeToDestroy < sbyte.MaxValue)
            {
                var ofst = 10F;
                Core.g.DrawLine(new Pen(Color.Red, 3F), ofst, Core.rh - ofst, ofst + (Core.rw - ofst) * (1F - TimeToDestroy / (float)sbyte.MaxValue), Core.rh - ofst);
            }
        }
    }
}
