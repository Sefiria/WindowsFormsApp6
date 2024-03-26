using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace SharedClasses
{
    [Serializable]
    public class User
    {
        static public Dictionary<string, User> Users = new Dictionary<string, User>();

        public long ZenScore, BombsScore, SurvivalMiniScore, SurvivalMaxScore, HitsScore, TimerScore, SwitchHitsScore, GlueScore;
        public float Rank;

        public User()
        {
            ZenScore = 0;
            BombsScore = 0;
            SurvivalMiniScore = 0;
            SurvivalMaxScore = 0;
            HitsScore = 0;
            TimerScore = 0;
            SwitchHitsScore = 0;
            GlueScore = 0;

            Rank = 0F;
        }
        public User(string msg)
        {
            string[] lines = msg.Split('\n');

            long GetValue(int i)
            {
                if (i < lines.Length && lines[i].Split(':').Length > 1)
                    return long.Parse(lines[i].Split(':')[1]);
                return 0;
            }

            string Name = lines[0];
            ZenScore = GetValue(1);
            BombsScore = GetValue(2);
            SurvivalMiniScore = GetValue(3);
            SurvivalMaxScore = GetValue(4);
            HitsScore = GetValue(5);
            TimerScore = GetValue(6);
            SwitchHitsScore = GetValue(7);
            GlueScore = GetValue(8);

            if (!Users.ContainsKey(Name))
                Users.Add(Name, this);

            if (Users[Name].ZenScore < ZenScore)
                Users[Name].ZenScore = ZenScore;
            if (Users[Name].BombsScore < BombsScore)
                Users[Name].BombsScore = BombsScore;
            if (Users[Name].SurvivalMiniScore < SurvivalMiniScore)
                Users[Name].SurvivalMiniScore = SurvivalMiniScore;
            if (Users[Name].SurvivalMaxScore < SurvivalMaxScore)
                Users[Name].SurvivalMaxScore = SurvivalMaxScore;
            if (Users[Name].HitsScore < HitsScore)
                Users[Name].HitsScore = HitsScore;
            if (Users[Name].TimerScore < TimerScore)
                Users[Name].TimerScore = TimerScore;
            if (Users[Name].SwitchHitsScore < SwitchHitsScore)
                Users[Name].SwitchHitsScore = SwitchHitsScore;
            if (Users[Name].GlueScore < GlueScore)
                Users[Name].GlueScore = GlueScore;
        }

        static public void Read()
        {
            if (!File.Exists("datastorage"))
                return;

            Users = new JavaScriptSerializer().Deserialize<Dictionary<string, User>>(File.ReadAllText("datastorage"));
        }
        static public void Write()
        {
            File.WriteAllText("datastorage", new JavaScriptSerializer().Serialize(Users));
        }
        static public string ToString()
        {
            string result = "";

            foreach(KeyValuePair<string, User> entry in Users)
            {
                result += string.Format("{0,-12}  {1, -10}  {2, -10}  {3, -10}  {4, -10}  {5, -10}  {6, -10}  {7, -10}  {8, -10}", entry.Key, entry.Value.ZenScore, entry.Value.BombsScore, entry.Value.SurvivalMiniScore, entry.Value.SurvivalMaxScore, entry.Value.HitsScore, entry.Value.TimerScore, entry.Value.SwitchHitsScore, entry.Value.GlueScore) + "\n";
            }

            return result;
        }
    }
}
