using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        // 1 file 1 type thing... not good
        internal static Dictionary<string, string> Load(string path)
        {
            Dictionary<string, string> errorList = new Dictionary<string, string>();
            string project_info = File.ReadAllText(Path.Combine(path, ""));
            try { Project = JsonSerializer.Deserialize<ProjectInfos>(project_info, Common.JsonOptions); } catch (Exception) { errorList["info.json[ROOT]"] = "Cannot Deserialize with type 'ProjectInfos'"; return errorList; }

            string[] files = Directory.GetFiles(Path.Combine(path, "prototypes"));
            string content;
            foreach (var file in files)
            {
                content = File.ReadAllText(file);
                var props = JsonSerializer.Deserialize<Dictionary<string, string>>(content, Common.JsonOptions);
                if (!props.ContainsKey("type")) { errorList[Path.GetFileName(file)] = "Missing property 'type'"; continue; }
                try
                {
                    switch (props["type"])
                    {
                        case "item": Items.Add(JsonSerializer.Deserialize<ItemPrototype>(content, Common.JsonOptions)); break;
                        case "recipe": Recipes.Add(JsonSerializer.Deserialize<RecipePrototype>(content, Common.JsonOptions)); break;
                        case "technology": Technologies.Add(JsonSerializer.Deserialize<Technology>(content, Common.JsonOptions)); break;
                        case "entity": break;
                        case "fluid": break;
                        case "tile": break;
                            // not even sure about these below in real prod
                        case "group": Groups.Add(JsonSerializer.Deserialize<Group>(content, Common.JsonOptions)); break;
                        case "subgroup": SubGroups.Add(JsonSerializer.Deserialize<SubGroup>(content, Common.JsonOptions)); break;

                        default: errorList[Path.GetFileName(file)] = "Unknown 'type'"; continue;
                    }
                }
                catch(Exception)
                {
                    errorList[Path.GetFileName(file)] = $"Cannot Deserialize with type '{props["type"]}'";
                    continue;
                }
            }
            return errorList;
        }
    }
}
