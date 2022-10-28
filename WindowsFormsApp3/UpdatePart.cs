using System;
using System.Collections.Generic;
using System.Diagnostics;
using WindowsFormsApp3.Entities;
using WindowsFormsApp3.Entities.Mobs;

namespace WindowsFormsApp3
{
    public static class UpdatePart
    {
        static Stopwatch WaveTime = new Stopwatch();

        public static void Init()
        {
            NextWave();
        }
        public static void Update(MainForm form)
        {
            var Entities = new List<DrawableEntity>(SharedData.Entities);
            foreach (var entity in Entities)
            {
                if (!entity.Exists)
                {
                    SharedData.Entities.Remove(entity);
                    continue;
                }

                if (entity.Exists)
                {
                    entity.Update();
                }
            }

            if (SharedData.Player.Exists)
                SharedData.Player.Update();

            if (WaveTime.Elapsed.Seconds >= 60)
            {
                NextWave();
            }
        }

        private static void NextWave()
        {
            SharedData.Wave++;
            WaveTime.Restart();
            float x, y;
            double a;
            for (int i = 0; i < SharedData.Wave * 20; i++)
            {
                a = Tools.RND.NextDouble() * 360D;
                x = SharedCore.RenderW / 2F + (float)(Math.Sin(Maths.DegToRad(a))) * SharedCore.RenderW * 0.4F;
                y = SharedCore.RenderH / 2F + (float)(Math.Cos(Maths.DegToRad(a))) * SharedCore.RenderH * 2F;
                new MobDummy(x, y);
            }
        }
    }
}
