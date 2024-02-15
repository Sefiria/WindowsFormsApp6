using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp24.Enumerations;
using System.Xml.Linq;
using WindowsFormsApp24.Properties;
using Tooling;

namespace WindowsFormsApp24.Events
{
    internal class ClosetContainer : EventContainer
    {
        internal static readonly int ClosetStackSizeX = 4;
        internal static readonly int ClosetStackSizeY = 3;

        internal ClosetContainer(int x, int y, int z) : base(ClosetStackSizeX, ClosetStackSizeY, x, y, z, NamedObjects.ClosetContainer) {}
        internal ClosetContainer(float x, float y, float z) : base(ClosetStackSizeX, ClosetStackSizeY, x, y, z, NamedObjects.ClosetContainer) {}

        internal override void PrimaryAction()
        {
            var p = Core.MainCharacter;
            if (p.HandObject.IsNotDefined() || Stack.Count >= StackSize)
                return;
            var ev = Map.GetEvent(p.HandObject);
            Stack.Add(ev.Guid);
            p.HandObject = Guid.Empty;
            var i = Stack.Count - 1;
            var x = i % ClosetStackSizeX;
            var y = i / ClosetStackSizeX;
            ev.X = X + x * W / StackSizeX;
            ev.Y = Y + y * H / StackSizeX;
            ev.Z = Z + 1;
            ev.AttachTo(this);
        }

        internal override void Update()
        {
            MouseHover = false;
            Highlight = Character.MainHandObjectDefined && StackIsNotFull;
            //if (Position == PrevPosition && !HasToResetStackPositions)
            //    return;
            PrevPosition = Position;
            var stack = new List<Guid>(Stack);
            Event ev;
            float i = -1, x, y;
            foreach (Guid guid in stack)
            {
                i++;
                ev = Map.GetEvent(guid);
                if (ev == null)
                {
                    Stack.Remove(guid);
                    continue;
                }
                x = i % ClosetStackSizeX;
                y = (int)(i / ClosetStackSizeX);
                if (y == 0) y = 44; else if (y == 1) y = 63; else if (y == 2) y = 84;
                ev.X = X + 10 + x * W / StackSizeX - ev.W / 2F;
                ev.Y = Y - H + y;
                ev.Z = Z + 1;
            }
        }
    }
}
