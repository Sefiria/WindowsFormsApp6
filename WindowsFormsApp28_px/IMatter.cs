using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp28_px
{
    public interface IMatter
    {
        float X { get; set; }
        float Y { get; set; }
        float A { get; set; }
        PointF Point { get; }
        List<Item> Inventory { get; set; }

        void Update();
        void Action(IMatter triggerer);
        void Draw(Graphics g);
        void DrawUI(Graphics g);

    }
}
