namespace Defacto_rio
{
    public class PropertyTypes
    {
        public class Property { }

        public class Ingredient : Property
        {
            public string type = "", name = "";
            public string amount = "";
            public Ingredient() { }
            public Ingredient(string type, string name, string amount)
            {
                this.type = type;
                this.name = name;
                this.amount = amount;
            }
        }
        public class Result : Ingredient { public Result() { } public Result(string type, string name, string amount) : base(type, name, amount) { } }
        public class Unit : Property
        {
            public string ingredients = "";
            public string count = "";
            public Unit() { }
            public Unit(string ingredients, string count)
            {
                this.ingredients = ingredients;
                this.count = count;
            }
        }
        public class Effect : Property
        {
            public string type = "";
            public string recipe = "";
            public Effect() { }
            public Effect(string type, string recipe)
            {
                this.type = type;
                this.recipe = recipe;
            }
        }
    }
}
