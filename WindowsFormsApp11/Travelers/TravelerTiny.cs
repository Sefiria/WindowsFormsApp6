using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Items;

namespace WindowsFormsApp11.Travelers
{
    public class TravelerTiny : Traveler
    {
        public override ItemPackage Loot { get; set; }

        public TravelerTiny() : base()
        {
            List<Item> items = new List<Item>()
            {
                ItemFactory.Create()
            };

            int count = Var.Rnd.Next(5);
            while(count > 0)
            {
                items.Add(ItemFactory.Create());
                count--;
            }

            Loot = new ItemPackage(items);
        }

        public override void Update()
        {
            base.Update();

            Y += SpeedMove;
        }
    }
}
