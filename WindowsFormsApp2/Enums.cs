using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;
using WindowsFormsApp2.Interfaces;

namespace WindowsFormsApp2
{
    public enum Items
    {
        Unknown,
        Coin,
        PlantAquarus,
        PlantSelanium,
        Knife,
    }
    public enum MaterialQuality
    {
        Unknown,
        Wood,
        Stone,
        Iron,
        Diamond,
    }

    public static class MaterialExt
    {
        public static int GetDamage(this IDamager damager)
        {
            switch(damager.Material)
            {
                case MaterialQuality.Unknown: return 0;
                case MaterialQuality.Wood:
                    switch(damager)
                    {
                        case Knife k: return 2;
                    }
                    break;
            }
            return 0;
        }
    }
}
