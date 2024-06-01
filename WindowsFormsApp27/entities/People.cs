using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp27.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WindowsFormsApp27.entities
{
    public class People : Entity
    {
        public float speed = 1.0F;
        public Tent Home = null;
        public bool AtHome => !Common.IsSimRuning && (xy_old, xy).LerpDistanceMin(Home?.xy) < 10F;
        public Guid TargetFruitID = Guid.Empty, TakingFruitID = Guid.Empty;
        public int FruitsOwn = 0;

        float take_fruit_cooldown = 0F;

        public static People Create(float x, float y, string dna)
        {
            var e = new People();

            e.type = 0;
            e.x = x;
            e.y = y;

            e.Image = Resources.ppl.Transparent();
            e.w = e.Image.Width; e.h = e.Image.Height;

            e.ApplyDna(dna);

            Entities.Add(e);
            return e;
        }
        protected override void ApplyDna(string dna)
        {
            base.ApplyDna(dna);
            byte[] dna_hair_rgb = GetDNAMinorFromMajor(dna, 0xA0, 3);
            byte[] dna_skin_rgb = GetDNAMinorFromMajor(dna, 0xA1, 3);
            byte[] dna_clothes_rgb = GetDNAMinorFromMajor(dna, 0xA2, 3);
            Color hair_color = Color.FromArgb(dna_hair_rgb[0], dna_hair_rgb[1], dna_hair_rgb[2]);
            Color skin_color = Color.FromArgb(dna_skin_rgb[0], dna_skin_rgb[1], dna_skin_rgb[2]);
            Color clothes_color = Color.FromArgb(dna_clothes_rgb[0], dna_clothes_rgb[1], dna_clothes_rgb[2]);
            Image = Image.ChangeColor(Color.FromArgb(255, 0, 0), hair_color)
                                   .ChangeColor(Color.FromArgb(0, 255, 0), skin_color)
                                   .ChangeColor(Color.FromArgb(0, 0, 255), clothes_color);
        }

        public override void Draw(Graphics g)
        {
            if(!AtHome)
                base.Draw(g);
        }

        public override void Update()
        {
            if (!exists)
                return;
            if (AtHome)
            {
                xy_old = xy;
                x = Home?.x ?? x;
                y += speed * Global.SPEED_SCALE;
            }
            else
            {
                behave();
            }
        }

        public void GoBackToHome()
        {
            if (Home == null || AtHome)
                return;

            xy_old = xy;
            var look = (Home.xy - xy).Normalized();
            x += look.x * speed * Global.SPEED_SCALE;
            y += look.y * speed * Global.SPEED_SCALE;
        }

        void behave()
        {
            if (TakingFruitID == Guid.Empty)
                do_behavior_chase_fruit();
            else
                do_behavior_take_fruit();
        }
        void do_behavior_chase_fruit()
        {
            if (Fruits.Count == 0)
                return;

            if (TargetFruitID != Guid.Empty)
            {
                var fruit = Fruits.First(f => f.ID == TargetFruitID);
                var d = (xy_old == null || xy_old == xy) ? xy.Distance(fruit.xy) : (xy_old, xy).LerpDistanceMin(fruit.xy);
                if (d < 10)
                {
                    TakingFruitID = TargetFruitID;
                    TargetFruitID = Guid.Empty;
                }
                else
                {
                    xy_old = xy;
                    var look = (fruit.xy - xy).Normalized();
                    x += look.x * speed * Global.SPEED_SCALE;
                    y += look.y * speed * Global.SPEED_SCALE;
                }
            }
            else
            {
                float _d = float.MaxValue, temp_d = float.MaxValue;
                Fruit closest = null;
                List<Fruit> fruits = new List<Fruit>(Fruits);
                if (Global.Settings.BOOL_PLAYER_AVOID_PROBLEMS)
                    fruits.RemoveAll(x => Peoples.Any(p => p.TakingFruitID == x.ID || p.TargetFruitID == x.ID));
                if (fruits.Count == 0 && Fruits.Count > 0)
                    fruits = Fruits;
                foreach (var fruit in fruits)
                {
                    temp_d = (xy_old == null || xy_old == xy) ? xy.Distance(fruit.xy) : (xy_old, xy).LerpDistanceMin(fruit.xy);
                    if (temp_d < _d)
                    {
                        _d = temp_d;
                        closest = fruit;
                    }
                }
                if (closest == null)
                    return;
                if (_d < 10)
                {
                    TakingFruitID = closest.ID;
                }
                else
                {
                    TargetFruitID = closest.ID;
                    xy_old = xy;
                    var look = (closest.xy - xy).Normalized();
                    x += look.x * speed * Global.SPEED_SCALE;
                    y += look.y * speed * Global.SPEED_SCALE;
                }
            }
        }
        void do_behavior_take_fruit()
        {
            if (take_fruit_cooldown < Global.FRUIT_TAKE_COOLDOWN_TICKS)
            {
                take_fruit_cooldown += Global.SPEED_SCALE;
            }
            else
            {
                take_fruit_cooldown = 0;
                var opponents = Peoples.Except(this).Where(x => x.exists && x.TakingFruitID == TakingFruitID || x.TargetFruitID == TakingFruitID).ToArray();
                if(opponents.Length == 0)
                {
                    var fruit = Fruits.FirstOrDefault(x => x.ID == TakingFruitID);
                    if(fruit != null)
                    {
                        FruitsOwn++;
                        fruit.exists = false;
                        TakingFruitID = Guid.Empty;
                    }
                }
                else
                {
                    var opponent = opponents[(int)(RandomThings.rnd1() * opponents.Length)];
                    opponent.Home.exists = false;
                    opponent.exists = false;
                    Common.sim_deaths++;
                }
            }
        }
        public override string dna_muted()
        {
            string muted_dna = dna;
            MuteDNAMajor(ref muted_dna, 0xA0, 3, 0x28);// hair
            MuteDNAMajor(ref muted_dna, 0xA1, 3, 0x08);// skin
            MuteDNAMajor(ref muted_dna, 0xA2, 3, 0x50);// clothes
            return muted_dna;
        }
    }
}
