using ConfigureRoute.Obj;
using System;
using System.Drawing;
using Tooling;
using static ConfigureRoute.Enumerations;
using static Tooling.RandomThings;

namespace ConfigureRoute.Entities
{
    public partial class Car : Entity
    {
        private class Data
        {
            public PointF look;
            public Road road;
            public Sign sign;
            public Data(PointF look, Road road)
            {
                this.look = look;
                this.road = road;
                sign = road?.Sign;
            }
        }

        public int Diagonal;
        public float Acceleration = 0.005F;
        public int Visibility = 3; // Nombre de tiles que la car peut observer devant elle
        public RangeValueF Speed = new RangeValueF(0F, 1F, 1F);
        public DrivingStatus Status = DrivingStatus.Accelerate;
        public override Rectangle Bounds => new Rectangle(Pos.MinusF((W / 2, H / 2).P()).ToPoint(), ((float)Diagonal).iSz());
        public Road RoadUnder() => Core.Map.FindRoad(Pos);
        public void PosAddLook()
        {
            var look = CalculateLook();
            Pos = Pos.PlusF(look.X * Speed.Value, look.Y * Speed.Value);
        }
        public void PosRemoveLook()
        {
            var look = CalculateLook();
            Pos = Pos.MinusF(look.x(0.5F));
        }
        public void PosAddLook(PointF look) => Pos = Pos.PlusF(look.x(Speed.Value));
        public void PosAddLook(PointF look, float speed) => Pos = Pos.PlusF(look.X * speed, look.Y * speed);
        public void SetStopWhenNoSpeed() { if (Math.Round(Speed.Value, 2) == 0) Status = DrivingStatus.Stop; }

        public Car()
            : base()
        {
            W = 18 - 2 + rnd( 11);
            H = 10 - 2 + rnd(11);
            int w = (int)W;
            int h = (int)H;
            int sz = Diagonal = (int)Maths.Diagonal(w, h);
            var img = new Bitmap(sz, sz);// cubic
            using(Graphics g = Graphics.FromImage(img))
            {
                Color c = rndbyte_color;
                Color cr = c.Reversed();
                g.FillRectangle(new SolidBrush(c), sz / 2 - w / 2, sz / 2 - h / 2, w, h);
                g.DrawRectangle(new Pen(cr), sz / 2 - w / 2, sz / 2 - h / 2, w, h);
                g.FillRectangle(new SolidBrush(cr), sz / 2 - w / 2 + w * 0.7F, sz / 2 - h / 2 + h *0.2F, w*0.2F, h*0.6F);
                g.FillRectangle(Brushes.Maroon, sz / 2 - w / 2 + 2, sz / 2 - h / 2 + 2, 4, 4);
                g.FillRectangle(Brushes.Maroon, sz / 2 - w / 2 + 2, sz / 2 - h / 2 + h - 2 - 4, 4, 4);
            }
            tex = img;
        }
        public override void Draw(Graphics g, PointF? position = null)
        {
            if (tex != null)
            {
                var tex = this.tex;
                if(Angle.Value != 0) tex = tex.Rotated(Angle.Value);
                if(Status == DrivingStatus.Brake || Status == DrivingStatus.HardBrake)
                    tex = tex.ChangeColor(Color.Maroon, Color.Red);
                if (Core.DeleteEntitiesMode)
                {
                    tex = new Bitmap(tex);
                    using (Graphics gtex = Graphics.FromImage(tex))
                    {
                        int sz = Diagonal;
                        gtex.FillRectangle(new SolidBrush(Color.FromArgb(Bounds.Contains(MouseStates.Position.PlusF(Core.Cam).ToPoint()) ? 200 : 50, Color.Red)), 0, 0, sz, sz);
                        gtex.DrawRectangle(new Pen(Color.Red, 2F), 0, 0, sz, sz);
                    }
                }
                g.DrawImage(tex, Pos.MinusF((W / 2, H / 2).P()).MinusF(Core.Cam));
            }
        }
        public override void Update()
        {
            base.Update();

            var road = RoadUnder();
            var look = CalculateLook();

            if (Focus == null)
                Status = DrivingStatus.Accelerate;

            observeAndBehave(look, road);
            statusmgt(look, road);
        }
        private void observeAndBehave(PointF look, Road road)
        {
            var data = new Data(look, road);

            if (road != null)
            {
                Analyse_DistanceAvecVoitureDevant(data);
                Analyse_ResteBienDansSaVoie(data);
                Analyse_VerifieSiDoitBientotTournerPourRalentir(data);
                Analyse_TourneSensRoute(data);
                Analyse_VerifieCederPassage(data);
            }
            else if (Status != DrivingStatus.Stop)
            {
                Status = DrivingStatus.HardBrake;
                SetStopWhenNoSpeed();
            }
            DoFocus(data);
        }
        void statusmgt(PointF look, Road road)
        {
            switch (Status)
            {
                case DrivingStatus.Stop: break;
                case DrivingStatus.HardBrake: Speed.Value -= Acceleration * 4F; PosAddLook(look); Angle.Value += -5 + rnd(7); SetStopWhenNoSpeed(); break;
                case DrivingStatus.Brake: Speed.Value -= Acceleration * 1.6F; PosAddLook(look); SetStopWhenNoSpeed(); break;
                case DrivingStatus.Decelerate: Speed.Value -= Acceleration / 2F; PosAddLook(look); SetStopWhenNoSpeed(); break;
                case DrivingStatus.Accelerate: Speed.Value += Acceleration; PosAddLook(look); break;
                case DrivingStatus.Hold: PosAddLook(look); break;
            }
        }
        float GetAngleGapWithRoad()
        {
            var road = RoadUnder();
            if (road == null) return 0F;
            var A = Angle.Value;
            var B = road.Angle;
            var _1 = B - A;
            var _2 = 360F + _1;
            return _1 * _1 < _2 * _2 ? _1 : _2;
        }
    }
}
