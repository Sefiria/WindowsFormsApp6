using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public class Prog
    {
        [JsonPropertyName("p")]
        public List<ProgRecord> ProgRecords { get; set; } = new List<ProgRecord>();

        private bool IsRecording = false;

        public Prog(){}

        public void RecStart(bool reset = true)
        {
            if (IsRecording) return;
            if (reset)
            {
                ProgRecords.Clear();
            }
            IsRecording = true;
            ManageKeyboard.OnKeyDown += ManageKeyboard_OnKeyDown;
            ManageKeyboard.OnKeyUp += ManageKeyboard_OnKeyUp;
        }
        public void RecStop()
        {
            if (!IsRecording) return;
            IsRecording = false;
            ManageKeyboard.OnKeyDown -= ManageKeyboard_OnKeyDown;
            ManageKeyboard.OnKeyUp -= ManageKeyboard_OnKeyUp;
        }
        private void ManageKeyboard_OnKeyDown(Keys key)
        {
            ProgRecords.Add(new ProgRecord(Core.MatchTime.ElapsedTicks, key, ProgRecord.KeyTypes.Down));
        }
        private void ManageKeyboard_OnKeyUp(Keys key)
        {
            ProgRecords.Add(new ProgRecord(Core.MatchTime.ElapsedTicks, key, ProgRecord.KeyTypes.Up));
        }

        private void ReOrderByDate()
        {
            ProgRecords = ProgRecords.OrderBy(x => x.Time).ToList();
        }

        public static void Save(Prog prog)
        {
            prog.ReOrderByDate();
            if (!Directory.Exists("progs/"))
                Directory.CreateDirectory("progs/");
            List<string> files = Directory.GetFiles("progs/", "prog_*.prog").ToList();
            List<int> filesN = files.Where(x => int.TryParse(Path.GetFileNameWithoutExtension(string.Concat(x.Skip(5))), out _)).Select(x => int.Parse(x)).ToList();
            int n = filesN.Count > 0 ? filesN.Max() : 0;
            string fn = $"prog_{n}.prog";
            string content = JsonSerializer.Serialize(prog, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText($"progs/{fn}", content);
        }
        public static Prog Load(int n)
        {
            string fn = $"progs/prog_{n}.prog";
            if (File.Exists(fn))
            {
                string file = File.ReadAllText(fn);
                Prog result = JsonSerializer.Deserialize<Prog>(file);
                result.ReOrderByDate();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
