using ConfigureRoute.Entities;
using ConfigureRoute.Obj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;
using static ConfigureRoute.Core;
using static ConfigureRoute.Enumerations;
using static Tooling.KB;

namespace ConfigureRoute
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        UserSettings Settings = new UserSettings();
        RouteTools Tool = RouteTools.Road;
        bool FirstClickHoldNotOnUI = false;

        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Core.Image);

            LoadUI();
        }
        private void Update(object _, EventArgs e)
        {
            //AnimMgr.Update();

            MgControls();

            UIMgt.MouseDown();
            if (MouseStates.IsDown) MouseClick();

            //if (KB.IsKeyPressed(KB.Key.E))
            //    Core.Inventory.Toggle();

            //if (Core.Inventory.IsOpen)
            //{
            //}
            //else
            //{
            Core.Map.Update();
            //}

            UIMgt.Update();

            KB.Update();
        }

        private void MgControls()
        {
            if (!Focused)
                return;
            if (IsKeyDown(Key.Z))
                Cam.Y -= CamSpeed;
            if (IsKeyDown(Key.S))
                Cam.Y += CamSpeed;
            if (IsKeyDown(Key.Q))
                Cam.X -= CamSpeed;
            if (IsKeyDown(Key.D))
                Cam.X += CamSpeed;
            if (IsKeyPressed(Key.LeftAlt))
                AltMode = !AltMode;
            if (IsKeyPressed(Key.R))
            {
                if (IsKeyDown(Key.LeftCtrl))
                    Direction = Direction == 0 ? 2 : (Direction == 1 ? 3 : (Direction == 2 ? 0 : 1));
                else
                    Direction += IsKeyDown(Key.LeftShift) ? 1 : -1;
            }
            if (IsKeyDown(Key.A) && !DeleteEntitiesMode)
            {
                if (Tool == RouteTools.Road)
                    Direction = Core.Map.GetTargetRoad()?.z ?? 0;
            }
            Road r;
            if (IsKeyPressed(Key.C) && !DeleteEntitiesMode)
            {
                if (IsKeyDown(Key.LeftCtrl))
                    Auto = !Auto;
                else if ((r = Core.Map.GetTargetRoad()) != null)
                    Core.Map.CreateCar(r);
            }
            if (IsKeyPressed(Key.Tab))
                DeleteEntitiesMode = !DeleteEntitiesMode;
            if (IsKeyDown(Key.LeftCtrl) && IsKeyPressed(Key.Supr))
            {
                Core.Map.Roads.Clear();
                Core.Map.Signs.Clear();
                Core.Map.Entities.Clear();
            }
        }

        private void Draw(object _, EventArgs e)
        {
            Core.g.Clear(Settings.BackgroundColor);
            Core.Map.Draw(g);
            DrawUI(Core.g);
            //if (Core.Inventory.IsOpen)
            //    Core.Inventory.Draw();

            Render.Image = Core.Image;

            Core.Ticks++;
        }
        private void DrawUI(Graphics g)
        {
            if (UIMgt.CurrentHover == null && !DeleteEntitiesMode)
            {
                var pos = TargetPoint;
                int w = (int)g.VisibleClipBounds.Width;
                int h = (int)g.VisibleClipBounds.Height;
                Bitmap img = new Bitmap(w, h);
                using (Graphics gui = Graphics.FromImage(img))
                {
                    Bitmap tex = null;
                    if (Tool == RouteTools.Road)
                    {
                        tex = new Bitmap(Cube, Cube);
                        using (Graphics gtex = Graphics.FromImage(tex))
                        {
                            if (MouseStates.IsDown)
                            {
                                switch (MouseStates.ButtonDown)
                                {
                                    case MouseButtons.Left: gtex.Clear(Color.FromArgb(150, Color.White)); break;
                                    case MouseButtons.Right: gtex.Clear(Color.FromArgb(50, Color.Blue)); break;
                                    case MouseButtons.Middle: gtex.Clear(Color.FromArgb(50, Color.Yellow)); break;
                                }
                            }
                            else
                            {
                                gtex.Clear(Color.FromArgb(50, Color.Black));
                            }
                        }
                    }
                    else if (Tool == RouteTools.Sign)
                    {
                        pos = pos.Minus(Cube / 2F);
                        tex = new Bitmap(Cube*2, Cube*2);
                        var pen = new Pen(Color.RoyalBlue, 2F);
                        using (Graphics gtex = Graphics.FromImage(tex))
                        {
                            switch (ClosestSignDirection)
                            {
                                default: break;
                                case 0: gtex.DrawRectangle(pen, Cube / 2, Cube / 2 - Cube / 8, Cube, Cube / 4); break;
                                case 1: gtex.DrawRectangle(pen, Cube / 2 - Cube / 8, Cube / 2, Cube / 4, Cube); break;
                                case 2: gtex.DrawRectangle(pen, Cube / 2, Cube + Cube / 2 - Cube / 8, Cube, Cube / 4); break;
                                case 3: gtex.DrawRectangle(pen, Cube + Cube / 2 - Cube / 8, Cube / 2, Cube / 4, Cube); break;
                            }
                        }
                    }
                    gui.DrawImage(tex, pos);
                }
                g.DrawImage(img, 0, 0);
            }

            string str = AltMode ? "AltMode " : "";
            if (Auto) str += "Auto ";
            switch (Direction) { case 0: str += "↑"; break; case 1: str += "←"; break; case 2: str += "↓"; break; case 3: str += "→"; break; }
            var strsz = g.MeasureString(str, Font);
            int mrg = 5;
            g.FillRectangle(Brushes.DarkGray, RW - strsz.Width - mrg*3, mrg, strsz.Width + mrg*2, strsz.Height + mrg*2);
            g.DrawString(str, Font, Brushes.Black, RW - strsz.Width - mrg * 2, mrg * 2);

            UIMgt.Draw(g);

            if (DeleteEntitiesMode)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Red)), 0, 0, RW, RH);
                g.DrawRectangle(new Pen(Color.Red, 10F), 0, 0, RW, RH);
            }
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
            UIMgt.MouseDown();
            FirstClickHoldNotOnUI = UIMgt.CurrentClicked == null;
            if (Tool != RouteTools.Sign)
            {
                MouseClick();
            }
            else if (!DeleteEntitiesMode)
            {
                if (FirstClickHoldNotOnUI && UIMgt.CurrentClicked == null)
                {
                    var pt = TargetPoint.PlusF(Cam).Div(Cube);
                    var SignDirection = ClosestSignDirection;
                    if (ClosestSignDirection == -1)
                        return;
                    if (SignDirection == 2) pt.Y++;
                    if (SignDirection == 3) pt.X++;
                    int direction = SignDirection == 2 ? 0 : (SignDirection == 3 ? 1 : SignDirection);
                    var sign = Core.Map.Signs.FirstOrDefault(s => s.x == pt.X && s.y == pt.Y);
                    if (MouseStates.ButtonDown == MouseButtons.Left)
                    {
                        if (sign == null)
                        {
                            sign = new Obj.Sign { x = pt.X, y = pt.Y, t = direction == 0 ? 1 : 0, l = direction == 1 ? 1 : 0 };
                            Core.Map.Signs.Add(sign);
                        }
                        else if (direction == 0) sign.t++;
                        else if (direction == 1) sign.l++;
                        if(sign.t == 0 && sign.l == 0)
                            Core.Map.Signs.Remove(sign);
                    }
                    else if (MouseStates.ButtonDown == MouseButtons.Right)
                    {
                        if (sign != null) Core.Map.Signs.Remove(sign);
                    }
                }
            }
            else
            {
                RemoveEntityUnderMouse();
            }
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.Position = PointToClient(MousePosition);
        }

        private void LoadUI()
        {
            int sz = 20;
            Bitmap btRoad = new Bitmap(sz, sz);
            using (Graphics _g = Graphics.FromImage(btRoad))
            {
                _g.Clear(Color.DarkGray);
                _g.FillRectangle(Brushes.Black, sz / 2 - (int)(sz * 0.1), 0, (int)(sz * 0.2), (int)(sz * 0.4));
                _g.FillRectangle(Brushes.Black, sz / 2 - (int)(sz * 0.1), (int)(sz * 0.6), (int)(sz * 0.2), sz-1);
            }
            Bitmap btSign = new Bitmap(sz, sz);
            using (Graphics _g = Graphics.FromImage(btSign))
            {
                _g.Clear(Color.DarkGray);
                _g.FillEllipse(Brushes.Red, 0, 0, sz, sz);
                _g.FillEllipse(Brushes.White, (int)(sz * 0.2), (int)(sz * 0.2), (int)(sz * 0.6), (int)(sz * 0.6));
                _g.DrawLine(new Pen(Color.Red, 2F), (int)(sz * 0.2), (int)(sz * 0.8), (int)(sz * 0.8), (int)(sz * 0.2));
            }

            var panel = new UIPanelSelection() { Position=(10, 10).Vf(), Size=(400, 40).Vf() };
            panel.Content.AddRange(new List<UI>
            {
                new UIButton() { Position = (10+(int)(sz*1.5F)*0, 10).Vf(), Size = (sz*1.25F, sz*1.25F).Vf(), Tex = btRoad, OnClick=()=>Tool=RouteTools.Road },
                new UIButton() { Position = (10+(int)(sz*1.5F)*1, 10).Vf(), Size = (sz*1.25F, sz*1.25F).Vf(), Tex = btSign, OnClick=()=>Tool=RouteTools.Sign },
            });

            UIMgt.UI.Add(panel);
        }

        private void MouseClick()
        {
            if (!DeleteEntitiesMode)
            {
                if (MouseStates.ButtonDown == MouseButtons.Left || MouseStates.ButtonDown == MouseButtons.Right)
                {
                    if (FirstClickHoldNotOnUI && UIMgt.CurrentClicked == null)
                    {
                        if (Tool == RouteTools.Road)
                        {
                            var pt = TargetPoint.PlusF(Cam).Div(Cube);
                            var road = Core.Map.Roads.FirstOrDefault(r => r.x == pt.X && r.y == pt.Y);
                            if (MouseStates.ButtonDown == MouseButtons.Left)
                            {
                                if (road == null) Core.Map.Roads.Add(new Obj.Road { x = pt.X, y = pt.Y, z = Direction });
                                else road.z = Direction;
                            }
                            else if (MouseStates.ButtonDown == MouseButtons.Right)
                            {
                                if (road != null) Core.Map.Roads.Remove(road);
                            }
                        }
                    }
                }
            }
            else
            {
                RemoveEntityUnderMouse();
            }
        }

        private void RemoveEntityUnderMouse()
        {
            Entity clicked_entity = Core.Map.Entities.FirstOrDefault(e => e.Bounds.Contains(MouseStates.Position.PlusF(Cam).ToPoint()));
            if (clicked_entity != null)
                clicked_entity.Exist = false;
        }
        private int ClosestSignDirection
        {
            get
            {
                var mc = MouseStates.Position.PlusF(Cam);
                var road_top = Road.At(mc.Minus(0, Cube));
                var road_left = Road.At(mc.Minus(Cube, 0));
                var road_under = Road.At(mc);
                var road_bottom = Road.At(mc.PlusF(0, Cube));
                var road_right = Road.At(mc.PlusF(Cube, 0));

                if (road_under == null || (road_top == null && road_left == null && road_bottom == null && road_right == null))
                    return -1;

                var t = (Cube / 2, 0).P();
                var l = (0, Cube / 2).P();

                var d_t = road_top == null ? float.PositiveInfinity : Maths.Distance(road_under.WorldPosition.pt.PlusF(t), mc);
                var d_l = road_left == null ? float.PositiveInfinity : Maths.Distance(road_under.WorldPosition.pt.PlusF(l), mc);
                var d_b = road_bottom == null ? float.PositiveInfinity : Maths.Distance(road_bottom.WorldPosition.pt.PlusF(t), mc);
                var d_r = road_right == null ? float.PositiveInfinity : Maths.Distance(road_right.WorldPosition.pt.PlusF(l), mc);

                var list = new List<float>() { d_t, d_l, d_b, d_r };
                return list.IndexOf(list.Aggregate((a, b) => a < b ? a : b));
            }
        }
    }
}
