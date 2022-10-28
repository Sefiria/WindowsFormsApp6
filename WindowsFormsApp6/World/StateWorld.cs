using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Structures;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateWorld : IState
    {
        public static readonly int BlocCount = Core.W / Core.TileSzBase;
        public int TileSz => Core.TileSzBase;
        public IBloc[,] Blocs { get; set; } = new IBloc[BlocCount, BlocCount];
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public List<Particule> Particules { get; set; } = new List<Particule>();

        public StateWorld()
        {
            for (int i = 0; i < BlocCount; i++)
            {
                for (int j = 0; j < BlocCount; j++)
                {
                    Blocs[i, j] = new BlocGrass(i, j);
                }
            }

            StaticImage = new Bitmap(Core.W, Core.H);
        }

        public void InitializeWorld()
        {
            ReplaceBloc(4, 4, new BlocGrass(4, 4), new StructureShop(4, 4));
        }

        public void Update()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Update();
        }

        public void DrawStatic()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Draw(StaticImage, TileSz);
        }

        public void Draw()
        {
            Core.g.DrawImage(StaticImage, 0, 0);

            var structure = Blocs[Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit()].OwnerStructure;
            if (structure == null)
            {
                Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap(), TileSz, TileSz);
            }
            else
            {
                Core.g.DrawRectangle(Pens.Cyan, structure.X.ToWorld(), structure.Y.ToWorld(), structure.W * TileSz, structure.H * TileSz);
            }
        }

        public void MouseDown(MouseEventArgs e)
        {
            var structure = Blocs[Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit()].OwnerStructure;
            if (structure != null)
            {
                structure.MouseDown();
            }
        }

        public void ReplaceBloc(int x, int y, IBloc bloc, IStructure structure = null)
        {
            if(Blocs[x, y].OwnerStructure?.X == x && Blocs[x, y].OwnerStructure?.Y == y)
                Blocs[x, y].OwnerStructure.RemoveStructureFromAllBlocks();
            Blocs[x, y] = bloc;
            structure.AddStructureToAllBlocks();
            Blocs[x, y].Draw(StaticImage, TileSz);
        }
    }
}
