namespace Framework
{
    public static class GlobalVariables
    {
        public enum Entity
        {
            Organic = 0,
            Material,
            Equipment
        }

        public enum Organic
        {
            Playable = 0,
            UnPlayable
        }
        public enum Material
        {
            Collectible = 0,
            Block,
            VolatileBlock,
            Door,
            Background,
            Bullet
        }
        public enum Equipment
        {
            Craftable = 0
        }

        public enum Playable
        {
            Player = 0
        }
        public enum UnPlayable
        {
            Buildable = 0,
            UnBuildable
        }
        public enum Collectible
        {
            CraftItem = 0
        }

        public enum Buildable
        {
            Part = 0,
            Behavior,
            Boss,
            SpecialMob
        }
        public enum UnBuildable
        {
            Mob = 0
        }


        public enum Instantiable
        {
            Collectible = 0,
            Block,
            VolatileBlock,
            Door,
            Background,
            Craftable,
            Player,
            Boss,
            SpecialMob,
            Mob,
            Bullet
        }
        public enum Uninstantiable
        {
            Behavior
        }
    }
}
