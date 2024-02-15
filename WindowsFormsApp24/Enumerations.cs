using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp24
{
    internal class Enumerations
    {
        internal enum SceneStates
        {
            TitleScreen = 0,
            Menu,
            Map
        }
        internal enum InputNames
        {
            Up=0,Left,Down,Right,
            Primary,Secondary,
            Ok,Cancel,
            Menu
        }
        internal enum EvLayer
        {
            Above=0,Same,Below
        }
        internal enum DrawingPart
        {
            Top=0, Bottom
        }
        internal enum PredefinedAction
        {
            TakeDrop=0, Plant, Loot,
        }
        internal enum NamedObjects
        {
            // plants
            Carrot=0, Onion, Potatoe, Healherb, Salad, Pepper, Tomatoe,
            // tools
            Shovel, WateringCan,
            // Miscs
            Bag, EventContainer
        }
        internal enum WatercanStats
        {
            MaxVolume=0, FlowRate, FlowLoss, EvaporationRate, Spread,
        }
    }
}
