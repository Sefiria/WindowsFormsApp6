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
using static Defacto_rio.PropertyTypes;

namespace Defacto_rio
{
    public static class Common
    {
        static JsonSerializerOptions options = new JsonSerializerOptions() { AllowTrailingCommas = true };

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
                dgv.Rows.Add(field.Name, field.GetValue(instanceValues));
            });
        }
        public static string CreateJson(this FieldInfo[] fields)
        {
            return $"[{{{string.Join(",", fields.Select(f => $@"""{f.Name}"":"""""))}}},]";
        }
        public static string ParseJson(this List<Dictionary<string, string>> data)
        {
            List<Dictionary<string, string>> intermediary = new List<Dictionary<string, string>>();
            data.ForEach(dict => { if (dict.Any(kv => kv.Value != null)) intermediary.Add(dict); });
            return JsonSerializer.Serialize(intermediary, options);
        }
        public static List<Dictionary<string, string>> ParseJson(this string json)
        {
            return JsonSerializer.Deserialize<List<Dictionary<string, string>>> (json, options);
        }
    }
}
