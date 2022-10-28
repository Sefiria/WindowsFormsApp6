using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp3.Properties;

namespace WindowsFormsApp3.Entities.Mobs
{
    public class MobDummy : MobBase
    {
        public float Speed = 0.4F + (Tools.RND.Next(500) - 250) / 1000F;
        Timer TimerTryAnotherWay = new Timer() { Enabled = true, Interval = 2000 };
        Vecf AnotherWayLook = Vecf.Empty;

        public MobDummy(float X = 0F, float Y = 0F) : base(Resources.mob_dummy.MadeTransparent(), X, Y)
        {
            HPMax = HP = 5;
            TimerTryAnotherWay.Tick += TimerTryAnotherWay_Tick;
            SetAnotherWay();
        }

        public override void Update()
        {
            Vecf pp = new Vecf(SharedData.Player.X, SharedData.Player.Y);
            Vecf Look = Maths.GetLook(pp.X, pp.Y, X, Y);
            var nextX = X + (Look.X + (Tools.RND.Next(2000) / 1000F) - 1F) * Speed;
            var nextY = Y + (Look.Y + (Tools.RND.Next(2000) / 1000F) - 1F) * Speed;
            var collider = SharedData.Entities.FirstOrDefault(x => x != this && x is MobBase && x.Collides(this, nextX, nextY));
            if (collider == null || this.IsCloserToPlayerThan(collider))
            {
                X = nextX;
                Y = nextY;
            }
            else
            {
                X += AnotherWayLook.X;
                Y += AnotherWayLook.Y;
            }
        }
        private void TimerTryAnotherWay_Tick(object sender, System.EventArgs e)
        {
            SetAnotherWay();
        }
        private void SetAnotherWay()
        {
            var rndx = Tools.RND.Next(100);
            var rndy = Tools.RND.Next(100);
            AnotherWayLook.X = (rndx < 50 ? 1 : -1) * Tools.RND.Next(1000) / 1000F * Speed;
            AnotherWayLook.Y = (rndy < 50 ? 1 : -1) * Tools.RND.Next(1000) / 1000F * Speed;
        }
    }
}
