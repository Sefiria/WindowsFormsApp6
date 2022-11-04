using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.World.Blocs;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateMine : IState
    {
        public static readonly int BlocCount = Core.W / 64;
        public int TileSz => 64;
        IBloc[,] Blocs { get; set; } = new IBloc[BlocCount, BlocCount];
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public IState PreviousState { get; set; } = null;
        public List<Particule> Particules { get; set; } = new List<Particule>();

        public StateMine()
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

        public void Update()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Update();


            var particules = new List<Particule>(Particules);
            foreach (var p in particules)
            {
                p.Update();
                if (p.Destroy)
                    Particules.Remove(p);
            }
        }

        public void DrawStatic()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Draw(StaticImage, TileSz);
        }

        public void Draw()
        {
            Core.g.DrawImage(StaticImage, 0, 0);
            Core.g.DrawRectangle(Pens.Cyan, Core.MousePosition.X.Snap(), Core.MousePosition.Y.Snap(), Core.TileSz, Core.TileSz);

            foreach (var p in Particules)
                p.Draw();
        }

        public void MouseDown(MouseEventArgs e)
        {
            int x = Core.MousePosition.X.ToUnit();
            int y = Core.MousePosition.Y.ToUnit();
            if (e.Button == MouseButtons.Left)
            {
                int radius = Data.Instance.StatInfo.Inventory.GetUsedPickaxe().Radius;
                int restingDamage = Data.Instance.StatInfo.Inventory.GetUsedPickaxe().GetQualitySTR() * radius;
                if (restingDamage <= 0)
                    return;

                void DoHit(int _x, int _y, int farpoint)
                {
                    if (_x < 0 || _y < 0 || _x >= Blocs.GetLength(0) || _y >= Blocs.GetLength(1))
                        return;

                    var hit = Blocs[_x, _y].Life;
                    Blocs[_x, _y].Life -= restingDamage - farpoint >= 0 ? restingDamage - farpoint : 0;
                    if(farpoint == 0)
                        restingDamage -= hit - Blocs[_x, _y].Life < 0 ? hit : hit - Blocs[_x, _y].Life;
                    if (Blocs[_x, _y].Life <= 0)
                    {
                        int la_yer = Blocs[_x, _y].Layer;
                        BlocGenerator.Explode(_x, _y, Blocs[_x, _y].Image, BlocGenerator.ExplosionSize.Big);
                        if (Blocs[_x, _y].Ore != null)
                            Data.Instance.StatInfo.OresCount[Blocs[_x, _y].Ore.OreType] += Blocs[_x, _y].Ore.Count;
                        Blocs[_x, _y] = BlocGenerator.Generate(_x, _y, la_yer + 1, withOre: true);
                        Blocs[_x, _y].Draw(StaticImage, TileSz);
                    }
                    else
                    {
                        BlocGenerator.Explode(x, y, Blocs[x, y].Image, BlocGenerator.ExplosionSize.Small);
                    }
                }

                int TimeOut = 0;
                do
                {
                    for (int _x = -radius; _x <= radius; _x++)
                    {
                        for (int _y = -radius; _y <= radius; _y++)
                        {
                            DoHit(x + _x, y + _y, Math.Abs(_x) + Math.Abs(_y));
                            if (restingDamage <= 0)
                                break;
                        }
                        if (restingDamage <= 0)
                            break;
                    }
                    //DoHit(x, y, 0);
                    //for (int i = 1; i <= radius; i++)
                    //{
                    //    DoHit(x - i, y, i);
                    //    DoHit(x + i, y, i);
                    //    DoHit(x, y - i, i);
                    //    DoHit(x, y + i, i);
                    //    DoHit(x - i, y - i, i);
                    //    DoHit(x + i, y + i, i);
                    //    DoHit(x - i, y + i, i);
                    //    DoHit(x + i, y - i, i);
                    //}
                    TimeOut++;
                }
                while (restingDamage > 0 && TimeOut < 1000);
            }
        }
    }
}
