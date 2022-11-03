using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Blocs.Consommables;
using WindowsFormsApp6.World.Entities;
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
        public List<ConsoBase> ConsoBlocs { get; set; } = new List<ConsoBase>();
        public List<Entity> Entities { get; set; } = new List<Entity>();

        public IStructure GetStructureAt(int x, int y) => Blocs[x, y].OwnerStructure;
        public ConsoBase GetConsoBlocAt(int x, int y) => ConsoBlocs.FirstOrDefault(n => n.X == x && n.Y == y);

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

            int x, y;
            IStructure structure;
            ConsoBase consobloc;
            for (int i=0; i<20; i++)
            {
                x = Tools.RND.Next(1, BlocCount);
                y = Tools.RND.Next(1, BlocCount);
                structure = GetStructureAt(x, y);
                consobloc = GetConsoBlocAt(x, y);
                if (structure == null && consobloc == null)
                    ConsoBlocs.Add(new ConsoTree(x, y));
                else
                    i--;
            }

            Entities.Add(new Unit(10 * Core.TileSz, 10 * Core.TileSz));
        }

        public void Update()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Update();

            foreach (Entity e in Entities)
                e.Update();
        }

        public void DrawStatic()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Draw(StaticImage, TileSz);
        }

        public void Draw()
        {
            Core.g.DrawImage(StaticImage, 0, 0);

            ConsoBlocs.ForEach(x => x.Draw());


            foreach (Entity e in Entities)
                e.Draw();


            var structure = GetStructureAt(Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit());
            var consobloc = GetConsoBlocAt(Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit());

            if(structure != null)
                Core.g.DrawRectangle(Pens.Cyan, structure.X.ToWorld(), structure.Y.ToWorld(), structure.W * TileSz, structure.H * TileSz);
            else if(consobloc != null)
                Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap() - (consobloc.GetImage().Height - Core.TileSz), consobloc.GetImage().Width, consobloc.GetImage().Height);
            else
                Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap(), TileSz, TileSz);
        }

        public void MouseDown(MouseEventArgs e)
        {
            var structure = Blocs[Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit()].OwnerStructure;
            if (structure != null)
            {
                structure.MouseDown();
                return;
            }

            var consobloc = GetConsoBlocAt(Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit());
            if (consobloc != null)
            {
                consobloc.Used = true;
                return;
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
