namespace Defacto_rio
{
    public class Technology : Prototype
    {
        public string icon;
        public string icon_size;
        public string units;
        public string effects;

        public override string ToString()
        {
            return name ?? "";
        }
    }
}
