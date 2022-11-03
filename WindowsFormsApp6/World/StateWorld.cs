using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Blocs.Consommables;
using WindowsFormsApp6.World.Entities;
using WindowsFormsApp6.World.Structures;
using WindowsFormsApp6.World.WorldResources;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateWorld : IState
    {
        public static readonly int BlocCount = Core.W / Core.TileSzBase;
        public int TileSz => Core.TileSzBase;
        public IBloc[,] Blocs { get; set; } = new IBloc[BlocCount, BlocCount];
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        [JsonIgnore] public IState PreviousState { get; set; } = null;
        [JsonIgnore] public List<Particule> Particules { get; set; } = new List<Particule>();
        [JsonIgnore] public List<ConsoBase> ConsoBlocs { get; set; } = new List<ConsoBase>();
        public List<Entity> Entities { get; set; } = new List<Entity>();
        [JsonIgnore] public Unit SelectedUnit = null;
        [JsonIgnore] public List<IUI> UI { get; set; } = new List<IUI>();

        public IStructure GetStructureAt(int x, int y) => Blocs[x, y].OwnerStructure;
        public ConsoBase GetConsoBlocAt(int x, int y) => ConsoBlocs.FirstOrDefault(n => n.X == x && n.Y == y);
        public Unit GetUnitAt(int x, int y) => Entities.FirstOrDefault(n => n is Unit && x >= n.X && y >= n.Y && x < n.X + n.ImageLeft.Width && y < n.Y + n.ImageLeft.Height) as Unit;
        public List<Unit> GetUnits() => Entities.Where(n => n is Unit).Cast<Unit>().ToList();

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
            ReplaceBloc(12, 6, new BlocGrass(12, 6), new StructureMine(12, 6));
            ReplaceBloc(8, 16, new BlocGrass(8, 16), new StructureFactory(8, 16));

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

            AddUI();
        }

        private void AddUI()
        {
            UI.Add(new UIButton(Resources.conso_tree.Transparent(), 10, 50));
            UI.Last().OnClick += (_s, _e) => { GetUnits().ForEach(n => SelectRNDTargetTree(n)); };
        }
        private void SelectRNDTargetTree(Unit unit)
        {
            if (unit.Target != null && unit.Target is ConsoTree)
                return;
            var targets = ConsoBlocs.Where(n => n is ConsoTree && !n.Used).ToList();
            foreach (var target in ConsoBlocs.Where(n => n is ConsoTree && !n.Used))
                if (GetUnits().Any(x => x.Target == target))
                    targets.Remove(target);
            if (targets.Count > 0)
            {
                (float d, int id) nearest = (float.MaxValue, -1);
                foreach(var target in targets)
                {
                    float d = Maths.Distance(unit.X, unit.Y, target.X.ToWorld(), target.Y.ToWorld());
                    if (d < nearest.d)
                        nearest = (d, targets.IndexOf(target));
                }
                if(nearest.id > -1)
                    unit.Target = targets.ElementAt(nearest.id);
            }
        }

        public void Update()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Update();
        }

        public void IdleUpdate()
        {
            Tick();

            foreach (Entity e in Entities)
                e.Update();

            var particules = new List<Particule>(Particules);
            foreach (var p in particules)
            {
                p.Update(0.005F);
                if (p.Destroy)
                    Particules.Remove(p);
            }
        }

        public void Tick()
        {
            ConsoBlocs.Where(x => x is IReGrowable).Cast<IReGrowable>().ToList().ForEach(x => x.Tick());
        }

        public void DrawStatic()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Draw(StaticImage, TileSz);
        }

        public void Draw()
        {
            // STATIC

            Core.g.DrawImage(StaticImage, 0, 0);


            // STRUCTURES

            var structure = GetStructureAt(Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit());
            var consobloc = GetConsoBlocAt(Core.MousePosition.X.ToUnit(), Core.MousePosition.Y.ToUnit());

            if(structure != null)
                Core.g.DrawRectangle(Pens.Cyan, structure.X.ToCurWorld(), structure.Y.ToCurWorld(), structure.W * TileSz, structure.H * TileSz);
            else if(consobloc != null)
                Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap() - (consobloc.GetImage().Height - Core.TileSz), consobloc.GetImage().Width, consobloc.GetImage().Height);
            else
                Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap(), TileSz, TileSz);

            //Core.g.DrawString(Core.MousePosition.X.ToString(), new Font("Arial", 15F), Brushes.Black, 10, Core.W - 50);
            //Core.g.DrawString(Core.MousePosition.Y.ToString(), new Font("Arial", 15F), Brushes.Black, 50, Core.W - 50);


            // ENTITIES

            foreach (Entity e in Entities)
                e.Draw(SelectedUnit == e ? true : false);


            // CONSOBLOCS

            ConsoBlocs.ForEach(x => x.Draw());


            // PARTICULES

            foreach (Particule p in Particules)
                p.Draw();


            // UI

            foreach (var ui in UI)
                ui.Draw();
        }

        public void MouseDown(MouseEventArgs e)
        {
            var clickedUI = UI.Where(i => e.X >= i.X && e.X < i.X + i.W && e.Y >= i.Y && e.Y < i.Y + i.H).ToList();
            if (clickedUI.Count > 0)
                clickedUI.ForEach(i => i.Clicked());


            int x = Core.MousePosition.X.ToUnit();
            int y = Core.MousePosition.Y.ToUnit();

            var consobloc = GetConsoBlocAt(x, y);
            if (consobloc != null && SelectedUnit != null)
            {
                SelectedUnit.Target = consobloc;
                return;
            }
            SelectedUnit = null;

            var structure = Blocs[x, y].OwnerStructure;
            if (structure != null)
            {
                structure.MouseDown();
                return;
            }

            var unit = GetUnitAt(Core.MousePosition.X, Core.MousePosition.Y);
            if (unit != null)
            {
                SelectedUnit = unit;
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
