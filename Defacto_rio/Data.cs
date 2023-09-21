using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Defacto_rio
{
    public class Data
    {
        public class ProjectInfos
        {
            public string name = $"NewMod{Common.Rnd.Next(10)}{Common.Rnd.Next(10)}";
            public string version = "0.0.0";
            public string title = "";
            public string author = "";
            public string factorio_version = "1.1.87";
            public string description = "";
            public ProjectInfos()
            {
            }
            public string ToJson()
            {
                return Common.CreateJson(GetType().GetFields(), true, this);
            }
        }

        public static ProjectInfos Project = new ProjectInfos();
        public static List<Group> Groups = new List<Group>();
        public static List<SubGroup> SubGroups = new List<SubGroup>();
        public static List<ItemPrototype> Items = new List<ItemPrototype>();
        public static List<RecipePrototype> Recipes = new List<RecipePrototype>();
        public static List<Technology> Technologies = new List<Technology>();

        internal static void CreatePrototypesFiles(string path)
        {
            void Write<T>(List<T> protos, string name) where T : Prototype
            {
                string content = "data:extend({" + Environment.NewLine;
                protos.ForEach(x =>
                {
                    string part = Common.CreateJson(x.GetType().GetFields(), true, x);
                    part = part.Replace("\"[{", "{{").Replace("}]\"", "}}");
                    part = string.Join(Environment.NewLine, part.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Select(l => $"  {l}"));
                    content += part + Environment.NewLine;
                });
                content += "}";
                File.WriteAllText(Path.Combine(path, $"{name}.lua"), content);
            }

            Write(Groups, "groups");
            Write(SubGroups, "subgroups");
            Write(Items, "items");
            Write(Recipes, "recipes");
            Write(Technologies, "techs");
        }

        //public static string CreateDataLua()
        //{
        //    return "";
        //}
    }
}
