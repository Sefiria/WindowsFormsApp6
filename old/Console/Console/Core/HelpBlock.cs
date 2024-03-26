using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _Console.Core
{
    public class HelpBlock
    {
        public enum Blocks
        {
            Floor = 0,
            Wall,
            Door,
            Key,
            Tree,
            CrackedTree,
            Wood,
            Axe,
            ChestKey,
            ChestAxe,
            Furnace,
            Charcoal,
            Bowl,
            WaterBowl,
            OilBowl,
            MatterBowl,
            Seed,
            BluePlant,
            GrayPlant,
            MatterPlant,
            GreenPlant,
            RedPlant,
            DarkPlant,
            MatterDrop
        }

        static public void Initialize()
        {
            var query = from t in Assembly.GetAssembly(typeof(Core.Blocks.IBlock)).GetTypes()
                    where t.IsClass && t.Namespace == "_Console.Core.Blocks" && t.Name != "BlockBase"
                    select t;

            BlocksInstances = new List<Core.Blocks.IBlock>();
            List<Type> types = query.ToList();
            foreach(Type type in types)
            {
                BlocksInstances.Add((Core.Blocks.IBlock)Activator.CreateInstance(type));
            }
        }
        static private List<Core.Blocks.IBlock> BlocksInstances;

        public static Blocks GetEnumFromId(int tilevalue)
        {
            if (tilevalue >= Enum.GetValues(typeof(Blocks)).Length)
                return Blocks.Floor;

            return (Blocks)tilevalue;
        }
        public static int GetIdFromEnum(Blocks block)
        {
            return (int)block;
        }

        public static Core.Blocks.IBlock GetBlockFromName(string name)
        {
            Blocks block = Blocks.Floor;
            Enum.TryParse(name, out block);
            return GetBlockFromEnum(block);
        }
        public static Core.Blocks.IBlock GetBlockFromId(int tilevalue) => GetBlockFromEnum(GetEnumFromId(tilevalue));
        public static Core.Blocks.IBlock GetBlockFromEnum(Blocks block)
        {
            Core.Blocks.IBlock result = BlocksInstances.FirstOrDefault(x => x.EnumValue == block);

            if (result == null)
                result = BlocksInstances.First(x => x.EnumValue == Blocks.Floor);

            return result;
        }
    }
}
