using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;
using WindowsCardsApp15.Properties;
using WindowsCardsApp15.Utilities;

namespace WindowsCardsApp15.data
{
    internal class CarteGenerique : Box, ICarte
    {
        public Bitmap Image = null;

        public string DATA;

        public void Draw()
        {
            if (Image == null)
            {
                Data data = JsonSerializer.Deserialize<Data>(DATA);
                Image = new Bitmap(Rsx.FromID(data.id), Core.CardW, Core.CardH);
                using (Graphics g = Graphics.FromImage(Image))
                {
                    foreach (var txt in data.texts)
                        g.DrawString(txt.t, Core.Font, new SolidBrush(Color.FromArgb(txt.c)), txt.x, txt.y);
                }
            }

            Core.g.DrawImage(Image, x, y);
        }

        public class Data
        {
            public byte id { get; set; }
            public List<DataText> texts { get; set; } = new List<DataText>();
            public Data(){}
        }

        public class DataText
        {
            public int x { get; set; }
            public int y { get; set; }
            public string t { get; set; }
            public int c { get; set; }
            public DataText(){}
        }
    }
}
