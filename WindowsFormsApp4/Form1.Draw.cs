using System;
using System.Drawing;
using WindowsFormsApp4.Properties;

namespace WindowsFormsApp4
{
    public partial class Form1
    {
        private void TimerDraw_Tick(object sender, EventArgs e)
        {
            var g = Data.g;

            g.Clear(Color.Black);
            Draw();

            Render.Image = RenderImage;
        }

        public void Draw()
        {
            Graphics g = Data.g;
            Carte Carte = Data.Carte;
            for (int j = 0; j < Carte.H; j++)
            {
                for (int i = 0; i < Carte.W; i++)
                {
                    int v = Carte.Map[j, i];
                    switch(v)
                    {
                        case 1:  g.DrawImage(Resources._1, i * Data.TileSz, j * Data.TileSz); break;
                        case 2: g.DrawImage(Resources._2, i * Data.TileSz, j * Data.TileSz); break;
                    }
                }
            }

            foreach(Pion pion in Data.Pions)
            {
                pion.Draw();
            }

            if(Data.RollingDices || Data.EndRollingDices)
            {
                if(Data.EndRollingDices)
                    Data.EndRollingDices = Data.RollingDices = false;

                for (int i = 0; i < Data.Dices.Count; i++)
                {
                    g.DrawImage(Tools.GetDiceResFromValue(Data.Dices[i]), Render.Width / 2 - 64, Render.Height / 2 - 64);
                }
            }
        }
    }
}
