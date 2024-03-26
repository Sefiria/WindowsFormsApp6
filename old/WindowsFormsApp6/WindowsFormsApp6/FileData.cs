using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    static public class FileData
    {
        static public long ZenScore = 0;
        static public long BombsScore = 0;
        static public long SurvivalMiniScore = 0;
        static public long SurvivalMaxScore = 0;
        static public long HitsScore = 0;
        static public long TimerScore = 0;
        static public long SwitchHitsScore = 0;
        static public long GlueScore = 0;

        static public string GetRaw()
        {
            return $"{MAIN.Instance.UserName}\n" +
                $"ZenMode:{ZenScore}\n" +
                $"BombsMode:{BombsScore}\n" +
                $"SurvivalMiniMode:{SurvivalMiniScore}\n" +
                $"SurvivalMaxMode:{SurvivalMaxScore}\n" +
                $"HitsMode:{HitsScore}\n" +
                $"TimerMode:{TimerScore}\n" +
                $"SwitchHitsMode:{SwitchHitsScore}\n" +
                $"GlueMode:{GlueScore}\n";
        }
        static public bool SaveData()
        {
            bool bestscore = false;
            switch(MAIN.Instance.Mode)
            {
                case ZenMode m: if (ZenScore < MAIN.Instance.Mode.Score) { bestscore = true; ZenScore = MAIN.Instance.Mode.Score; } break;
                case BombsMode m: if (BombsScore < MAIN.Instance.Mode.Score) { bestscore = true; BombsScore = MAIN.Instance.Mode.Score; } break;
                case SurvivalMiniMode m: if (SurvivalMiniScore < MAIN.Instance.Mode.Score) { bestscore = true; SurvivalMiniScore = MAIN.Instance.Mode.Score; } break;
                case SurvivalMaxMode m: if (SurvivalMaxScore < MAIN.Instance.Mode.Score) { bestscore = true; SurvivalMaxScore = MAIN.Instance.Mode.Score; } break;
                case HitsMode m: if (HitsScore < MAIN.Instance.Mode.Score) { bestscore = true; HitsScore = MAIN.Instance.Mode.Score; } break;
                case TimerMode m: if (TimerScore < MAIN.Instance.Mode.Score) { bestscore = true; TimerScore = MAIN.Instance.Mode.Score; } break;
                case SwitchHitsMode m: if (SwitchHitsScore < MAIN.Instance.Mode.Score) { bestscore = true; SwitchHitsScore = MAIN.Instance.Mode.Score; } break;
                case GlueMode m: if (GlueScore < MAIN.Instance.Mode.Score) { bestscore = true; GlueScore = MAIN.Instance.Mode.Score; } break;
            }

            string raw = GetRaw();
            string data = Encrypt(raw);

            File.WriteAllText(Directory.GetCurrentDirectory() + "/data", data);

            return bestscore;
        }
        static public void SaveZenState(ZenMode mode)
        {
            string data = "";
            for (int x = 0; x < Grid.BlockCount; x++)
            {
                for (int y = 0; y < Grid.BlockCount; y++)
                {
                    data += (mode.Grid.grid[x][y].V + x + y).ToString() + " ";
                }
                data += "\n";
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/zenstatedata", data);
        }

        static public void LoadData()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/data"))
                return;

            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "/data");
            string raw = Decrypt(data);
            string[] lines = raw.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            long GetValue(int i)
            {
                if (i < lines.Length)
                    return long.Parse(lines[i].Split(':')[1]);
                return 0;
            }

            MAIN.Instance.UserName = lines[0];
            ZenScore = GetValue(1);
            BombsScore = GetValue(2);
            SurvivalMiniScore = GetValue(3);
            SurvivalMaxScore = GetValue(4);
            HitsScore = GetValue(5);
            TimerScore = GetValue(6);
            SwitchHitsScore = GetValue(7);
            GlueScore = GetValue(8); 
        }
        static public Grid LoadZenState()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/zenstatedata"))
                return null;

            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "/zenstatedata");
            string[] rows = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            Grid Grid = new Grid(rows.Length);

            for (int y = 0; y < Grid.BlockCount; y++)
            {
                string[] cells = rows[y].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < Grid.BlockCount; x++)
                {
                    Grid.grid[y][x].V = int.Parse(cells[x]) - y - x;
                }
                data += "\n";
            }

            return Grid;
        }

        static public string Encrypt(string raw)
        {
            string result = "";
            for (int i = 0, incr = 0; i < raw.Length; i++)
                result += (i % 2 == 0 ? raw[i] + 42 + incr++ : raw[i] + 128 - incr) + " ";
            return result;
        }
        static public string Decrypt(string raw)
        {
            string result = "";
            List<string> rawArray = raw.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<char> chars = new List<char>();
            rawArray.ForEach(x => chars.Add((char)int.Parse(x)));
            for (int i = 0, incr = 0; i < chars.Count; i++)
                result += (char) (i % 2 == 0 ? chars[i] - 42 - incr++ : chars[i] - 128 + incr);
            return result;
        }
    }
}
