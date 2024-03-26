using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Scoreboard : Form
    {
        public Scoreboard()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Net.SendMsg("Scoreboard");
        }

        internal void UpdateData(string msg)
        {
            DGV.Rows.Clear();

            var Users = new Dictionary<string, (long ZenScore, long BombsScore, long SurvivalMiniScore, long SurvivalMaxScore, long HitsScore, long TimerScore, long SwitchHitsScore, long GlueScore)>();
            string[] users = msg.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < users.Length; i++)
            {
                string[] row = users[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string Name = row[0];
                long ZenScore = long.Parse(row[1]);
                long BombsScore = long.Parse(row[2]);
                long SurvivalMiniScore = long.Parse(row[3]);
                long SurvivalMaxScore = long.Parse(row[4]);
                long HitsScore = long.Parse(row[5]);
                long TimerScore = long.Parse(row[6]);
                long SwitchHitsScore = long.Parse(row[7]);
                long GlueScore = long.Parse(row[8]);

                if (!Users.ContainsKey(Name))
                    Users.Add(Name, (ZenScore, BombsScore, SurvivalMiniScore, SurvivalMaxScore, HitsScore, TimerScore, SwitchHitsScore, GlueScore));
            }

            foreach (var user in Users)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(DGV, user.Key, user.Value.ZenScore, user.Value.BombsScore, user.Value.SurvivalMiniScore, user.Value.SurvivalMaxScore, user.Value.HitsScore, user.Value.TimerScore, user.Value.SwitchHitsScore, user.Value.GlueScore);
                DGV.Rows.Add(row);
            }

            HighlightBestScores();
        }

        private void HighlightBestScores()
        {
            for (int i=1; i< DGV.ColumnCount; i++)
            {
                List<DataGridViewRow> BestScoreRows = new List<DataGridViewRow>();
                long best = 0;
                foreach (DataGridViewRow row in DGV.Rows)
                {
                    long score = long.Parse(row.Cells[i].Value.ToString());
                    if (best <= score)
                    {
                        if (best < score)
                        {
                            BestScoreRows.Clear();
                            best = score;
                        }

                        BestScoreRows.Add(row);
                    }
                }

                foreach (DataGridViewRow exaequo in BestScoreRows)
                {
                    exaequo.Cells[i].Style.BackColor = BestScoreRows.Count == 1 ? Color.Yellow : Color.Cyan;
                    exaequo.Cells[i].Style.ForeColor = Color.Red;
                    exaequo.Cells[i].Style.Font = new Font(DefaultFont, FontStyle.Bold);
                }
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
