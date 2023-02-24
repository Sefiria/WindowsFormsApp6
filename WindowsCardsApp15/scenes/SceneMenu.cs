using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.UI;
using WindowsCardsApp15.utilities;

namespace WindowsCardsApp15.scenes
{
    internal class SceneMenu : Scene
    {
        public override void Initialize()
        {
            IUI pb;
            UIButton b;

            b = new UIButton("Nouvelle partie");
            b.X = RelativeCoords.M - b.W / 2;
            b.Y = RelativeCoords.B - 100;
            UI.Add(b);
            UI.Last().OnClick += NouvellePartie;
            pb = b;

            b = new UIButton("Quitter");
            b.X = RelativeCoords.M - b.W / 2;
            b.Y = pb.Y + pb.H + 10;
            UI.Add(b);
            UI.Last().OnClick += Quitter;
            pb = b;
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            DrawUI();
        }

        protected override void DrawUI()
        {
            UI.ForEach(x => x.Draw());
        }


        public void NouvellePartie(object _, EventArgs e)
        {
            Core.Scene = new SceneNouvellePartie();
        }

        public void Quitter(object _, EventArgs e)
        {
            Core.Quitter = true;
        }
    }
}
