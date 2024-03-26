using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class AttackWaitOpponent : Mode
    {
        public class FullUser
        {
            public string Name;
            public IPEndPoint IEP;

            public FullUser(string Name, IPEndPoint IEP)
            {
                this.Name = Name;
                this.IEP = IEP;
            }
            public FullUser(string Name, string IEP)
            {
                this.Name = Name;
                this.IEP = IEP == "self" ? null : new IPEndPoint(IPAddress.Parse(IEP.Split(':')[0]), int.Parse(IEP.Split(':')[1]));
            }
            public override string ToString()
            {
                return Name;
            }
        }
        int room = 0;
        FullUser Self = new FullUser("Me", "self");
        FullUser Opponent = null;

        public AttackWaitOpponent()
        {
            InitializeComponent();

            listUsers.Items.Add(Self);
            Net.SendMsg("GetUsers|Full");
        }
        public override void GetUsers(string[] users)
        {
            List<FullUser> items = new List<FullUser>();
            foreach (string user in users)
                if(user.Split('-')[0] != MAIN.Instance.UserName)
                    items.Add(new FullUser(user.Split('-')[0], user.Split('-')[1]));

            if(items.Count > 0)
            Invoke(new Action(() =>
            {
                listUsers.Items.Clear();
                listUsers.Items.Add(Self);
                listUsers.Items.AddRange(items.ToArray());
            }));
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            Net.SendMsg("GetUsers|Full");
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btAskDuel.Enabled = listUsers.SelectedIndex > -1 && (listUsers.SelectedItem as FullUser).Name != "Me";
        }

        private void btAskDuel_Click(object sender, EventArgs e)
        {
            Opponent = (listUsers.SelectedItem as FullUser);
            Net.SendMsg($"AttackP2P_AskDuel|{MAIN.Instance.UserName}", Opponent.IEP);
        }
        public void NetAskDuel(string msg, IPEndPoint source)
        {
            DialogResult result = MessageBox.Show($"{msg} is asking you a Duel!, Would you challenge him ?", "New Duel", MessageBoxButtons.YesNo);
            Net.SendMsg($"AttackP2P_ReplyDuel|{MAIN.Instance.UserName},{(result == DialogResult.Yes ? "Y" : "N")}", source);
            if (result == DialogResult.Yes)
            {
                Opponent = new FullUser(msg, source);
                AllUsersReady();
            }
        }
        public void NetDuelAnswer(string msg, IPEndPoint source)
        {
            if (msg.Split(',')[1] == "Y")
            {
                MessageBox.Show($"{msg.Split(',')[0]} has accepted the Duel!\nGet ready.", "New Duel");
                Opponent = new FullUser(msg, source);
                AllUsersReady();
            }
        }
        private void AllUsersReady()
        {
            Invoke(new Action(() =>
            {
                MAIN.Instance.Mode = new Attack(Opponent.Name, Opponent.IEP);
                MAIN.Instance.Mode.Initialize();
                MAIN.Instance.Mode.Show();
                Close();
                MAIN.Instance.Hide();
            }));
        }
    }
}
