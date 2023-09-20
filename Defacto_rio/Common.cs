using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using static Defacto_rio.PropertyTypes;

namespace Defacto_rio
{
    public static class Common
    {
        static JsonSerializerOptions options = new JsonSerializerOptions() { AllowTrailingCommas = true };
        public static Random Rnd = new Random((int)DateTime.UtcNow.Ticks);

        public static void Create(this DataGridView dgv, Type dataType)
        {
            dataType.GetFields().ToList().ForEach(field =>
            {
                dgv.Rows.Add(field.Name, "");
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = field.FieldType.GenericTypeArguments[0].GetFields().CreateJson();
            });
        }
        public static void Edit(this DataGridView dgv, Type dataType, object instanceValues)
        {
            dataType.GetFields().ToList().ForEach(field =>
            {
                dgv.Rows.Add(field.Name, field.GetValue(instanceValues) ?? "");
            });
        }
        public static string CreateJson(this FieldInfo[] fields, bool truefile = false, object instance_data = null)
        {
            if(truefile)
                return $@"{{
  {string.Join($",{Environment.NewLine}  ", fields.Select(f => $@"""{f.Name}"": ""{f.GetValue(instance_data)}"""))}
}}";
            return $"[{{{string.Join(",", fields.Select(f => $@"""{f.Name}"":"""""))}}},]";
        }
        public static string ParseJson(this List<Dictionary<string, string>> data)
        {
            List<Dictionary<string, string>> intermediary = new List<Dictionary<string, string>>();
            data.ForEach(dict => { if (dict.Any(kv => kv.Value != null)) intermediary.Add(dict); });
            return JsonSerializer.Serialize(intermediary, options);
        }
        public static List<Dictionary<string, string>> ParseJson(this string json_src)
        {
            if(json_src == null) return null;
            string json = json_src;
            if (json.Length == 0) return new List<Dictionary<string, string>>();
            if (json[0] != '{') json.Prepend('{');
            if (json.Last() != '}') json.Append('}');
            if (!json.Contains(':')) json = $@"{{"""":""{string.Concat(json)}""}}";

            try
            {
                return JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json, options);
            }
            catch (JsonException) { }

            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json, options);
                var result = new List<Dictionary<string, string>>() { data };
                return result;
            }
            catch (JsonException) { }

            return new List<Dictionary<string, string>>();
        }


        public static T ToPrototype<T>(this DataGridView dgv) where T : Prototype
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => { if(row.Cells[0].Value != null && row.Cells[1].Value != null) typeof(T).GetField(row.Cells[0].Value as string).SetValue(result, row.Cells[1].Value as string); });
            return result;
        }


        public static string GetTemplateJson(string template)
        {
            var type = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name == template).First();
            string json = type.GetFields().CreateJson();
            return json;
        }
    }
}
