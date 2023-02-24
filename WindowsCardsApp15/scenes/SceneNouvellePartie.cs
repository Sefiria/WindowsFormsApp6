using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.data;
using WindowsCardsApp15.data.genericdata;
using WindowsCardsApp15.UI;
using WindowsCardsApp15.utilities;

namespace WindowsCardsApp15.scenes
{
    internal class SceneNouvellePartie : Scene
    {
        CarteGenerique ClasseGuerrier = new CarteGenerique() { DATA = classesdata.DATA_GUERRIER };
        CarteGenerique ClasseMage = new CarteGenerique() { DATA = classesdata.DATA_MAGE };
        CarteGenerique ClasseRogue = new CarteGenerique() { DATA = classesdata.DATA_ROGUE };
        List<CarteGenerique> cartes;

        public override void Initialize()
        {
            ClasseGuerrier.x = 50; ClasseGuerrier.y = 50;
            ClasseMage.x = 50 + (Core.CardW + 10) * 1; ClasseMage.y = 50;
            ClasseRogue.x = 50 + (Core.CardW + 10) * 2; ClasseRogue.y = 50;

            cartes = new List<CarteGenerique>()
            {
                ClasseGuerrier,
                ClasseMage,
                ClasseRogue
            };
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            cartes.ForEach(x => x.Draw());
            DrawUI();
        }

        protected override void DrawUI()
        {
            UI.ForEach(x => x.Draw());
        }
    }
}
