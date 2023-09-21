namespace Defacto_rio
{
    public class RecipePrototype : Prototype
    {
        public string ingredients;
        public string results;
        public string energy_required;
        public string category;
        public string group;
        public string subgroup;
        public string order;

        public override string ToString()
        {
            return name ?? "";
        }
    }
}
