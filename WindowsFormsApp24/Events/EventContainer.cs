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
    internal class EventContainer : Event
    {
        internal NamedObjects NamedObject = NamedObjects.EventContainer;
        internal int StackSizeX, StackSizeY;
        internal int StackSize => StackSizeX * StackSizeY;
        internal List<Guid> Stack = new List<Guid>();
        internal Point PrevPosition = Point.Empty;
        internal bool HasToResetStackPositions = false;

        internal EventContainer(int stacksize_x, int stacksize_y, int x, int y, int z) : base(x, y, z)
        {
            Initialize(stacksize_x, stacksize_y);
        }
        internal EventContainer(int stacksize_x, int stacksize_y, float x, float y, float z) : base(x, y, z)
        {
            Initialize(stacksize_x, stacksize_y);
        }

        private void Initialize(int stacksize_x, int stacksize_y)
        {
            Name = "EventContainer";
            StackSizeX = stacksize_x;
            StackSizeY = stacksize_y;
            var img = Core.NamedTextures[NamedObject];
            TextureOffset.Y = 4F;
            Bounds = new RectangleF(0, -img.Height / 3 - TextureOffset.Y, img.Width, img.Height);
            Image = Core.NamedTextures[NamedObject];
            Image.MakeTransparent(Color.White);
        }

        internal override void PrimaryAction()
        {
            var p = Core.MainCharacter;
            if (p.HandObject.IsNotDefined() || Stack.Count >= StackSize)
                return;
            var ev = Map.GetEvent(p.HandObject);
            Stack.Add(ev.Guid);
            p.HandObject = Guid.Empty;
            var hw = W / ((StackSizeX + 2) * (float)ev.W) * ev.W - 2;
            var hh = H / ((StackSizeY + 2) * (float)ev.H) * ev.H - Core.TileSize - 2;
            ev.X = X + hw + ((Stack.Count-1) % StackSizeX) * (W / ((StackSizeX + 2) * (float)ev.W)) * ev.W;
            ev.Y = Y + hh + ((Stack.Count - 1) / StackSizeX) * (H / ((StackSizeY + 2) * (float)ev.H)) * ev.H;
            ev.Z = Z + 1;
            ev.AttachTo(this);
        }
        internal override void DetachChild(Event child)
        {
            Stack.Remove(child.Guid);
            HasToResetStackPositions = true;
        }

        internal override void Update()
        {
            MouseHover = false;
            Highlight = Character.MainHandObjectDefined;
            if (Position == PrevPosition && !HasToResetStackPositions)
                return;
            PrevPosition = Position;
            var stack = new List<Guid>(Stack);
            Event ev;
            float i = -1, sx = StackSizeX, sy = StackSizeY, hw, hh;
            foreach (Guid guid in stack)
            {
                i++;
                ev = Map.Current.Events.FirstOrDefault(e => e.Guid == guid);
                if (ev == null)
                {
                    Stack.Remove(guid);
                    continue;
                }
                hw = W / ((sx + 2) * ev.W) * ev.W - 2;
                hh = H / ((sy + 2) * ev.H) * ev.H - Core.TileSize - 2;
                ev.X = X + hw + (i%sx) * (W / ((sx+2) * ev.W)) * ev.W;
                ev.Y = Y + hh + (int)(i / sx) * (H / ((sy + 2) * ev.H)) * ev.H;
                ev.Z = Z + 1;
            }
        }
    }
    internal class BigContainer : EventContainer
    {
        internal static readonly int BigContainerStackSizeX = 6;
        internal static readonly int BigContainerStackSizeY = 3;

        internal BigContainer(int x, int y, int z) : base(BigContainerStackSizeX, BigContainerStackSizeY, x, y, z){}
        internal BigContainer(float x, float y, float z) : base(BigContainerStackSizeX, BigContainerStackSizeY, x, y, z){ }
    }
    internal class MediumContainer : EventContainer
    {
        internal static readonly int MediumContainerStackSizeX = 4;
        internal static readonly int MediumContainerStackSizeY = 3;

        internal MediumContainer(int x, int y, int z) : base(MediumContainerStackSizeX, MediumContainerStackSizeY, x, y, z) { }
        internal MediumContainer(float x, float y, float z) : base(MediumContainerStackSizeX, MediumContainerStackSizeY, x, y, z) { }
    }
    internal class SmallContainer : EventContainer
    {
        internal static readonly int SmallContainerStackSizeX = 3;
        internal static readonly int SmallContainerStackSizeY = 2;

        internal SmallContainer(int x, int y, int z) : base(SmallContainerStackSizeX, SmallContainerStackSizeY, x, y, z) { }
        internal SmallContainer(float x, float y, float z) : base(SmallContainerStackSizeX, SmallContainerStackSizeY, x, y, z) { }
    }
}
