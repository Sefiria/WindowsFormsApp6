using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Tooling;
using WindowsFormsApp24.Properties;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Event
    {
        internal int ID => Map.Current.Events.IndexOf(this);
        internal Guid Guid = Guid.NewGuid();
        internal string Name = "Unnamed";
        internal float X, Y, Z;
        internal int TileX => (int)(X / Core.TileSize);
        internal int TileY => (int)(Y / Core.TileSize);
        internal int TileZ => (int)(Z / Core.TileSize);
        internal Point Position => (X, Y).iP();
        internal Point TilePosition => (TileX, TileY).iP();
        internal RectangleF Bounds;
        internal PointF TextureOffset = PointF.Empty;
        internal int W => Frames?.Length > 0 ? Frames[0].Width : Image?.Width ?? 0;
        internal int H => Frames?.Length > 0 ? Frames[0].Height : Image?.Height ?? 0;
        internal RectangleF RealTimeBounds => new RectangleF(X + Bounds.X - Core.Cam.X, Y + Bounds.Y - Core.Cam.Y, Bounds.Width, Bounds.Height);
        internal bool IsOnScreen => Map.Current.IsEventOnScreen(this);
        internal bool Exists = true;
        internal string Filename = "";
        internal Bitmap Image = null;
        internal Bitmap[] Frames;
        internal int Direction = 0, Frame = 0;
        internal bool IsMoving = false;
        internal float MaxSpeed = 1.5F, MoveSpeed = 0F;
        internal EvLayer Layer = EvLayer.Same;
        internal bool MouseHover = false; // Manual set in UIMouseAssist.cs
        internal bool Highlight = false; // Manual set in Map.cs
        internal delegate void PrimaryActionHandler();
        internal delegate void SecondaryActionHandler();
        internal event PrimaryActionHandler OnPrimaryAction;
        internal event SecondaryActionHandler OnSecondaryAction;
        internal event PrimaryActionHandler OnPrimaryActionDown;
        internal event SecondaryActionHandler OnSecondaryActionDown;
        internal Dictionary<string, object> Data = new Dictionary<string, object>();

        internal List<Command> Acts = new List<Command>();
        internal int ActIndex = 0;
        internal bool ActsRepeat = false;
        internal Event AttachSource = null;

        private int frame_control = 0, frame_direction = 1;

        internal Event(int x, int y, int z = 0)
        {
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
            Z = z * Core.TileSize;
        }
        internal Event(float x, float y, float z = 0F)
        {
            X = x;
            Y = y;
            Z = z;
        }
        internal Event(string filename, int x, int y, int z = 0)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                LoadImage((Bitmap)System.Drawing.Image.FromFile(Filename));
                Bounds = new RectangleF(0, 0, Image.Width / 3, Image.Height / 4);
            }
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
            Z = z * Core.TileSize;
        }
        internal Event(string filename, float x, float y, float z = 0F)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                LoadImage((Bitmap)System.Drawing.Image.FromFile(Filename));
                Bounds = new RectangleF(0, 0, Image.Width / 3, Image.Height / 4);
            }
            X = x;
            Y = y;
            Z = z;
        }
        internal Event(Bitmap image, int x, int y, int z = 0)
        {
            LoadImage(image);
            Bounds = new RectangleF(0, 0, Image.Width / 3, Image.Height / 4);
            TextureOffset.Y = 4F;
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
            Z = z * Core.TileSize;
        }
        internal Event(Bitmap image, float x, float y, float z = 0F)
        {
            LoadImage(image);
            Bounds = new RectangleF(0, 0, Image.Width / 3, Image.Height / 4);
            TextureOffset.Y = 4F;
            X = x;
            Y = y;
            Z = z;
        }
        internal Event(Bitmap image, object unique_image_tag, int x, int y, int z = 0)
        {
            LoadImage(image, unique_image_tag);
            Bounds = new RectangleF(0, 0, Image.Width, Image.Height);
            if(unique_image_tag != null) TextureOffset.Y = 4F;
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
            Z = z * Core.TileSize;
        }
        internal Event(Bitmap image, object unique_image_tag, float x, float y, float z = 0F)
        {
            LoadImage(image, unique_image_tag);
            Bounds = new RectangleF(0, 0, Image.Width, Image.Height);
            if (unique_image_tag != null) TextureOffset.Y = 4F;
            X = x;
            Y = y;
            Z = z;
        }

        internal void LoadImage(Bitmap image, object unique_image_tag = null)
        {
            image.MakeTransparent(Color.White);
            Image = image;
            if(unique_image_tag == null)
                Frames = Image.Split(3, 4).ToArray();
        }
        internal void AttachTo(Event attachSource)
        {
            AttachSource = attachSource;
        }
        internal bool IsAttached => AttachSource != null;
        internal bool IsNotAttached => !IsAttached;
        internal void DetachSource()
        {
            AttachSource?.DetachChild(this);
            AttachSource = null;
        }
        internal virtual void DetachChild(Event child){}

        internal virtual void Update()
        {
            MouseHover = Highlight = false;

            if (ActIndex < Acts.Count || ActsRepeat)
            {
                Acts[ActIndex].Execute();
                ActIndex++;
                if (ActIndex == Acts.Count && ActsRepeat)
                    ActIndex = 0;
            }

            // Override before call base.Update();
            update_frame();
        }
        private void update_frame()
        {
            if (IsMoving && Frames != null)
            {
                if (frame_control > 10)
                {
                    frame_control = 0;
                    Frame += frame_direction;
                    if (Frame == -1) frame_direction = 1;
                    if (Frame == 1) frame_direction = -1;
                }
                else frame_control++;
            }
            else
            {
                Frame = frame_control = 0;
                frame_direction = 1;
            }
        }

        internal virtual void Draw()
        {
            var Cam = Core.Cam;
            var img = new Bitmap(Frames != null ? Frames[Direction * 3 + 1 + Frame] : Image);
            var map = Map.Current;
            img = map.DrawingPart == DrawingPart.Bottom ? BottomPartOf(img) : TopPartOf(img);
            if (img == null)
                return;
            if((MouseHover || Highlight) && Core.MainCharacter.HandObject != Guid) img = img.GetAdjusted(brightness:1F + 0.2F * ((Core.Instance.Ticks + 15) % 30) / 30F - 0.2F * (Core.Instance.Ticks % 30) / 30F);
            if(map.DrawingPart == DrawingPart.Top)
                Core.Instance.g.DrawImage(img, Position.PlusF(TextureOffset).Minus(0, img.Height).Minus(Cam.Position));
            else
                Core.Instance.g.DrawImage(img, Position.PlusF(TextureOffset).Minus(Cam.Position));
        }
        internal virtual void DrawExtraInfos(){}
        internal Bitmap BottomPartOf(Bitmap img) => img.Clone(new Rectangle(0, img.Height - Core.TileSize / 2, img.Width, Core.TileSize / 2), img.PixelFormat);
        internal Bitmap TopPartOf(Bitmap img) => img.Height - Core.TileSize / 2 <= 0 ? null : img.Clone(new Rectangle(0, 0, img.Width, img.Height - Core.TileSize / 2), img.PixelFormat);

        static internal bool Collides(Event caller, Event other, float lx, float ly)
        {
            var _lx = lx == 0F ? 0F : lx + Math.Sign(lx);
            var _ly = ly == 0F ? 0F : ly + Math.Sign(ly);
            RectangleF a = new RectangleF(caller.X+_lx+caller.Bounds.X, caller.Y+_ly+caller.Bounds.Y, caller.Bounds.Width, caller.Bounds.Height);
            RectangleF b = new RectangleF(other.X+other.Bounds.X, other.Y+other.Bounds.Y, other.Bounds.Width, other.Bounds.Height);
            return a.IntersectsWith(b);
        }
        static internal List<Event> CollidingObjects(Event caller, float LX, float LY, EvLayer[] layers, bool onlyFirstContact = false, Event[] exceptions = null)
        {
            List<Event> contacts = new List<Event>();

            var events = new List<Event>(Map.Current.Events)
                                .Where(ev => ev.Guid != Core.MainCharacter.HandObject)
                                .Where(ev => exceptions == null ? true : !exceptions.Contains(ev))
                                .Where(ev => ev.RealTimeBounds.X > caller.RealTimeBounds.X - 64 && ev.RealTimeBounds.X < caller.RealTimeBounds.X + 64 && ev.RealTimeBounds.Y > caller.RealTimeBounds.Y - 64 && ev.RealTimeBounds.Y < caller.RealTimeBounds.Y + 64)
                                .ToList();

            if (onlyFirstContact)
            {
                var contact = events.FirstOrDefault(ev => ev != caller && layers.Contains(ev.Layer) && Collides(caller, ev, LX, LY));
                if(contact != null)
                    contacts.Add(contact);
            }
            else
            {
                events.ForEach(ev => { if (ev != caller && layers.Contains(ev.Layer) && Collides(caller, ev, LX, LY)) contacts.Add(ev); });
            }

            return contacts;
        }
        static internal bool DoesntCollides(Event caller, float LX, float LY, EvLayer[] layers) => CollidingObjects(caller, LX, LY, layers, true).Count == 0;
        internal List<Event> CollidingObjects(float LX, float LY, bool onlyFirstContact = false) => CollidingObjects(this, LX, LY, new EvLayer[] { EvLayer.Same }, onlyFirstContact);
        internal List<Event> CollidingObjects(float LX, float LY, EvLayer layer, bool onlyFirstContact = false) => CollidingObjects(this, LX, LY, new EvLayer[] { layer }, onlyFirstContact);
        internal List<Event> CollidingObjects(float LX, float LY, EvLayer[] layers, bool onlyFirstContact = false) => CollidingObjects(this, LX, LY, layers, onlyFirstContact);
        internal bool DoesntCollides(float LX, float LY) => DoesntCollides(this, LX, LY, new EvLayer[] { EvLayer.Same });
        internal bool DoesntCollides(float LX, float LY, EvLayer layer) => DoesntCollides(this, LX, LY, new EvLayer[] { layer });
        internal bool DoesntCollides(float LX, float LY, EvLayer[] layers) => DoesntCollides(this, LX, LY, layers);

        internal virtual void PrimaryAction()
        {
            OnPrimaryAction?.Invoke();
        }
        internal virtual void SecondaryAction()
        {
            OnSecondaryAction?.Invoke();
        }
        internal virtual void PrimaryActionDown()
        {
            OnPrimaryActionDown?.Invoke();
        }
        internal virtual void SecondaryActionDown()
        {
            OnSecondaryActionDown?.Invoke();
        }

        internal static Action<Event> PredefinedAction_TakeDrop = (Event caller) =>
        {
            var data = caller.Data;
            var p = Core.MainCharacter;
            if (p.HandObject.IsNotDefined())
            {
                if (caller.IsAttached)
                    caller.DetachSource();
                p.HandObject = caller.Guid;
            }
            else if (p.HandObject == caller.Guid)
            {
                p.HandObject = Guid.Empty;
                var pt = ((float)data["X"], (float)data["Y"]).P();
                var prevX = pt.X;
                var prevY = pt.Y;
                caller.X = pt.X;
                caller.Y = pt.Y;
                PointF look = PointF.Empty;
                while (Collides(caller, p, 0F, 0F))
                {
                    if (look == PointF.Empty)
                        look = caller.Position.Minus(p.Position).ToPointF().norm();
                    caller.X += look.X;
                    caller.Y += look.Y;
                }
                if (CollidingObjects(caller, 0F, 0F, new[] { EvLayer.Same }, true, new[] { p }).Count != 0)
                {
                    p.HandObject = caller.Guid;
                    caller.X = prevX;
                    caller.Y = prevY;
                }
            }
        };
        internal static Action<Event> PredefinedAction_Plant = (Event caller) =>
        {
            if (caller.Name.EndsWith("-seed"))
            {
                var map = Map.Current;
                var x = (float)caller.Data["X"];
                var y = (float)caller.Data["Y"];
                if (map.IsCrop((int)(x /Core.TileSize), (int)(y / Core.TileSize)) && !map.Events.Any(ev => ev.TilePosition == (x,y).P().Div(Core.TileSize) && ev is Crop))
                {
                    caller.X = x;
                    caller.Y = y;
                    Core.MainCharacter.HandObject = Guid.Empty;
                    caller.Exists = false;
                    NamedObjects namedObj;
                    caller.Name = caller.Name.Replace("-seed", "");
                    if (Enum.TryParse(caller.Name, out namedObj))
                        map.Events.Add(new Crop(namedObj, caller.TileX, caller.TileY) { total_time_to_grow = (long)caller.Data["grow_ticks"] });
                }
            }
        };
        internal static Action<Event> PredefinedAction_Loot = (Event caller) =>
        {
            var data = caller.Data;
            var p = Core.MainCharacter;
            if (p.HandObject.IsNotDefined())
            {
                bool hasName = caller.Data.ContainsKey("Name");
                string name = "";
                if(hasName)
                    name = (string)data["Name"];
                if (hasName & name.EndsWith("-seed"))
                {
                    name = name.Replace("-seed", "");
                    NamedObjects namedObj;
                    if (Enum.TryParse(name, out namedObj))
                        Map.Current.Events.Add(new Seed(namedObj, (long)data["grow_ticks"], 0F, 0F));
                }
                else
                {
                    if(caller.Data.ContainsKey("Name"))
                        Map.Current.Events.Add((Event)caller.Data["Name"]);
                    else
                    {
                        Map.Current.Events.Add(new Event((Bitmap)data["Image"], true, 0, 0));
                        if (hasName)
                            Map.Current.Events.Last().Name = name;
                    }
                }
                p.HandObject = Map.Current.Events.Last().Guid;
            }
        };
        internal static Dictionary<PredefinedAction, Action<Event>> PredefinedActions = new Dictionary<PredefinedAction, Action<Event>>()
        {
            [PredefinedAction.TakeDrop] = PredefinedAction_TakeDrop,
            [PredefinedAction.Plant] = PredefinedAction_Plant,
            [PredefinedAction.Loot] = PredefinedAction_Loot,
        };
    }
}
