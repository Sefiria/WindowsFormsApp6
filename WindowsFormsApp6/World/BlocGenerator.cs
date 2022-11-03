using System;
using System.Drawing;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World
{
    public static class BlocGenerator
    {
        public enum ExplosionSize
        {
            Small = 0,
            Medium,
            Big
        }

        public static void Explode(int x, int y, Bitmap Image, ExplosionSize explosionSize)
        {
            int rndWithinTile() => Tools.RND.Next(Data.Instance.State.TileSz);

            if ((int)explosionSize > 0)
            {
                new Particule(x.ToCurWorld(rndWithinTile()), y.ToCurWorld(rndWithinTile()), -3F, -3F, Image);
                new Particule(x.ToCurWorld(rndWithinTile()), y.ToCurWorld(rndWithinTile()), -3F, 3F, Image);
                new Particule(x.ToCurWorld(rndWithinTile()), y.ToCurWorld(rndWithinTile()), 3F, -3F, Image);
                new Particule(x.ToCurWorld(rndWithinTile()), y.ToCurWorld(rndWithinTile()), 3F, 3F, Image);
            }

            int min, max;
            switch(explosionSize)
            {
                default:
                case ExplosionSize.Small: min = 1; max = 2; break;
                case ExplosionSize.Medium: min = 4; max = 10; break;
                case ExplosionSize.Big: min = 12; max = 24; break;
            }

            var rnd = Tools.RND.Next(min, max);
            for(int i=0; i< rnd; i++)
                new Particule(x.ToCurWorld(rndWithinTile()), y.ToCurWorld(rndWithinTile()), Tools.RND.Next(70) / 10F - 3F, Tools.RND.Next(70) / 10F - 3F, Image);
        }

        public static IBloc Generate(int x, int y, int layer, bool withOre = false)
        {
            int grass = 1;
            int dirt = 3;
            int deepdirt = 6;
            int stone = 12;

            bool validateWithOre = false;
            IBloc result;

            if (layer <= grass)
                return new BlocGrass(x, y, layer);

            if (layer <= dirt)
                return new BlocDirt(x, y, layer);

            if (layer <= deepdirt)
                return new BlocDeepDirt(x, y, layer);

            if (withOre) validateWithOre = true;

            if (layer <= stone)
                result = new BlocStone(x, y, layer);

            //throw new ArgumentException("layer unexpected");
            result = new BlocStone(x, y, layer);

            if(validateWithOre)
            {
                OreType oreType = Ore.GetOre(layer);
                if (oreType != OreType.None)
                {
                    int count = Tools.RND.Next(Enum.GetNames(typeof(OreType)).Length - (int)oreType);
                    if (count > 0)
                    {
                        result.Ore = new Ore(oreType, count);
                    }
                }
            }

            return result;
        }
    }
}
