using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp17.Utilities;
using static WindowsFormsApp17.enums;

namespace WindowsFormsApp17
{
    internal class UserUI
    {
        sbyte TimeToDestroy = sbyte.MaxValue;
        List<object> PossibleCrafts = new List<object>();
        int hand_selection = 0;
        float cam_speed = 4F;

        public UserUI()
        {
        }


        public void MouseWheel(MouseEventArgs e)
        {
            hand_selection += (e.Delta > 0 ? 1 : -1);
            if (hand_selection >= PossibleCrafts.Count) hand_selection = 0;
            if (hand_selection < 0) hand_selection = PossibleCrafts.Count - 1;
            //InHand = PossibleCrafts[hand_selection] == null ? null : (Structure)Activator.CreateInstance(PossibleCrafts[hand_selection].GetType());
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
            if (e.Button == MouseButtons.Right)
                Data.Instance.map.reset(Core.MouseCamTile.x, Core.MouseCamTile.y);
            //if (e.Button == MouseButtons.Right && Data.Instance.ThereStructureAt(Core.MouseSnap))
            //{
            //    if (TimeToDestroy <= 0)
            //    {
            //        Data.Instance.Structures.Remove(Core.MouseSnap);
            //        TimeToDestroy = sbyte.MaxValue;
            //    }
            //    else
            //    {
            //        var s = Data.Instance.GetStructureAt(Core.MouseSnap);
            //        TimeToDestroy -= (sbyte)(s is StructureConveyor ? 10 : (s is IMoveInfos ? 2 : 1));
            //    }
            //}
            //else
            //    TimeToDestroy = sbyte.MaxValue;
        }
        private void AddStructure(MouseEventArgs e)
        {
        //    if (InHand == null)
        //        return;
        //    if (e.Button == MouseButtons.Left && (!Data.Instance.ThereStructureAt(Core.MouseSnap) || Data.Instance.GetStructureAt(Core.MouseSnap) is StructureConveyor))
        //    {
        //        object structure;
        //        var rotationStructures = new List<Type> {
        //            typeof(StructureConveyor),
        //            typeof(StructureDrill),
        //            typeof(StructureCompressor),
        //            typeof(StructureLiquefer) };
        //        if (rotationStructures.Contains(PossibleCrafts[hand_selection].GetType()))
        //            structure = Activator.CreateInstance(PossibleCrafts[hand_selection].GetType(), new object[] { Core.MouseSnap, Rotation });
        //        else
        //            structure = Activator.CreateInstance(PossibleCrafts[hand_selection].GetType(), new object[] { Core.MouseSnap });
        //        if (Data.Instance.ThereStructureAt(Core.MouseSnap))
        //            Data.Instance.Structures.Remove(Core.MouseSnap);
        //        Data.Instance.Structures.Add(Core.MouseSnap, (Structure)structure);
        //    }
        }
        public void Update()
        {
            //void setinhand(KB.Key k, int n) { if (KB.IsKeyPressed(k)) { hand_selection = n; InHand = PossibleCrafts[hand_selection] == null ? null : (Structure)Activator.CreateInstance(PossibleCrafts[hand_selection].GetType()); } }
            //setinhand(KB.Key.Num1, 0);
            //setinhand(KB.Key.Num2, 1);
            //setinhand(KB.Key.Num3, 2);
            //setinhand(KB.Key.Num4, 3);
            //setinhand(KB.Key.Num5, 4);

            if (KB.IsKeyDown(KB.Key.Q))
                Data.Instance.cam.x -= cam_speed;
            if (KB.IsKeyDown(KB.Key.D))
                Data.Instance.cam.x += cam_speed;
            if (KB.IsKeyDown(KB.Key.Z))
                Data.Instance.cam.y -= cam_speed;
            if (KB.IsKeyDown(KB.Key.S))
                Data.Instance.cam.y += cam_speed;
        }

        public void Display()
        {
            //if (InHand != null)
            //{
            //    InHand.vec = Core.MouseVec;
            //    InHand.Display();
            //    InHand.FrontDisplay();

            //    Core.g.DrawImage(arrows[(Way)Rotation], Core.MouseVec.snap(Core.TSZ).pt);
            //}

            //if (TimeToDestroy < sbyte.MaxValue)
            //{
            //    var ofst = 10F;
            //    Core.g.DrawLine(new Pen(Color.Red, 3F), ofst, Core.rh - ofst, ofst + (Core.rw - ofst) * (1F - TimeToDestroy / (float)sbyte.MaxValue), Core.rh - ofst);
            //}
        }
    }
}
