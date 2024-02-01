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
        internal string Name = "Unnamed";
        internal float X, Y;
        internal int TileX => (int)(X / Core.TileSize);
        internal int TileY => (int)(Y / Core.TileSize);
        internal Point Position => (X, Y).iP();
        internal Point TilePosition => (TileX, TileY).iP();
        internal PointF Offset = PointF.Empty;
        internal Rectangle OffsetTexture = Rectangle.Empty;
        internal int W => Frames?.Length > 0 ? Frames[0].Width : Image?.Width ?? 0;
        internal int H => Frames?.Length > 0 ? Frames[0].Height : Image?.Height ?? 0;
        internal RectangleF Bounds => new RectangleF(X+Offset.X-Core.Cam.X, Y+Offset.Y-Core.Cam.Y, W, H);
        internal bool Exists = true;
        internal string Filename = "";
        internal Bitmap Image = null;
        internal Bitmap[] Frames;
        internal int Direction = 0, Frame = 0;
        internal bool IsMoving = false;
        internal float MaxSpeed = 1.5F, MoveSpeed = 0F;
        internal EvLayer Layer = EvLayer.Same;
        internal bool MouseHover = false; // Manual set in UIMouseAssist.cs
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

        private int frame_control = 0, frame_direction = 1;

        internal Event(float x, float y)
        {
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
        }
        internal Event(string filename, float x, float y)
        {
            Filename = filename;
            if (File.Exists(Filename))
                LoadImage((Bitmap)System.Drawing.Image.FromFile(Filename));
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
        }
        internal Event(Bitmap image, float x, float y)
        {
            LoadImage(image);
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
        }
        internal Event(Bitmap image, object unique_image_tag, float x, float y)
        {
            LoadImage(image, unique_image_tag);
            X = x * Core.TileSize;
            Y = y * Core.TileSize;
        }

        internal void LoadImage(Bitmap image, object unique_image_tag = null)
        {
            image.MakeTransparent(Color.White);
            Image = image;
            if(unique_image_tag == null)
                Frames = Image.Split(3, 4).ToArray();
        }

        internal virtual void Update()
        {
            MouseHover = false;

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
            if(MouseHover && Core.MainCharacter.HandObject != ID) img = img.GetAdjusted(brightness:1F + 0.2F * ((Core.Instance.Ticks + 15) % 30) / 30F - 0.2F * (Core.Instance.Ticks % 30) / 30F);
            if(map.DrawingPart == DrawingPart.Top)
                Core.Instance.g.DrawImage(img, Position.PlusF(Offset).Minus(0, img.Height).Minus(Cam.Position));
            else
                Core.Instance.g.DrawImage(img, Position.PlusF(Offset).Minus(Cam.Position));
        }
        internal Bitmap BottomPartOf(Bitmap img) => img.Clone(new Rectangle(0, img.Height - Core.TileSize / 2, img.Width, Core.TileSize / 2), img.PixelFormat);
        internal Bitmap TopPartOf(Bitmap img) => img.Height - Core.TileSize / 2 <= 0 ? null : img.Clone(new Rectangle(0, 0, img.Width, img.Height - Core.TileSize / 2), img.PixelFormat);

        static internal bool Collides(Event caller, Event other, float lx, float ly)
        {
            var _lx = lx == 0F ? 0F : lx + Math.Sign(lx);
            var _ly = ly == 0F ? 0F : ly + Math.Sign(ly);
            var w = Core.TileSize;
            var h = w / 2;
            RectangleF a = new RectangleF(caller.X+h+_lx+caller.OffsetTexture.X, caller.Y+_ly+caller.OffsetTexture.Y, w, h);
            RectangleF b = new RectangleF(other.X+h+other.OffsetTexture.X, other.Y+other.OffsetTexture.Y, w, h);
            return a.IntersectsWith(b);
        }
        static internal List<Event> CollidingObjects(Event caller, float LX, float LY, EvLayer[] layers, bool onlyFirstContact = false)
        {
            List<Event> contacts = new List<Event>();

            var events = new List<Event>(Map.Current.Events)
                                .Where(ev => ev.X > caller.X - 64 && ev.X < caller.X + 64)
                                .Where(ev => ev.Y > caller.Y - 64 && ev.Y < caller.Y + 64)
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
            if (p.HandObject == -1)
            {
                p.HandObject = caller.ID;
            }
            else if (p.HandObject == caller.ID)
            {
                p.HandObject = -1;
                var pt = ((float)data["X"], (float)data["Y"]).P();
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
            }
        };
        internal static Action<Event> PredefinedAction_Plant = (Event caller) =>
        {
            PredefinedAction_TakeDrop(caller);
            if (caller.Name.EndsWith("-seed"))
            {
                var map = Map.Current;
                if (map.IsCrop(caller.TileX, caller.TileY))
                {
                    caller.Exists = false;
                    NamedObjects namedObj;
                    caller.Name = caller.Name.Replace("-seed", "");
                    if (Enum.TryParse(caller.Name, out namedObj))
                        map.Events.Add(new Crop(Core.NamedTextures[namedObj], 3, 3) { total_time_to_grow = (long)caller.Data["grow_ticks"] });
                }
            }
        };
        internal static Action<Event> PredefinedAction_Loot = (Event caller) =>
        {
            var data = caller.Data;
            var p = Core.MainCharacter;
            if (p.HandObject == -1)
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
                p.HandObject = Map.Current.Events.Last().ID;
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
