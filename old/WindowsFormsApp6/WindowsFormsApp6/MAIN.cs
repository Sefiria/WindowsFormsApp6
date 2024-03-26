using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class MAIN : Form
    {
        static public MAIN Instance = null;
        public Mode Mode = null;
        public string UserName = "Toto";
        Timer PingServer = new Timer() { Enabled = false, Interval = 2000 };
        Scoreboard Scoreboard = null;
        Chat Chat = null;

        public MAIN()
        {
            InitializeComponent();

            Instance = this;
            FileData.LoadData();
            textBox1.Text = UserName;

            Chat = new Chat();
            Chat.Show();

            Net.UDP.BeginReceive(new AsyncCallback(ClientReceiveLoop), Net.UDP);

            PingServer.Tick += delegate
            {
                Invoke(new Action(() => Icon = Properties.Resources.ServerDown));
                if (Mode != null)
                    Invoke(new Action(() => Mode.Icon = Properties.Resources.ServerDown));
                Net.SendMsg($"ping|{UserName}");
            };
            PingServer.Start();

            Block.InitializeCombos();
            Net.SendMsg($"Login|{UserName}");
        }

        public void SelectMode(object sender, EventArgs e)
        {
            Application.DoEvents();
            Mode = null;
            Type type = null;
            if (sender is Mode)
                type = sender.GetType();
            else
            {
                string ButtonName = (sender as Button).Text.Replace(" ", "");
                type = Type.GetType("WindowsFormsApp6." + (ButtonName == "Attack!" ? "AttackWaitOpponent" : ButtonName + "Mode"));
            }
            Mode = Activator.CreateInstance(type) as Mode;
            Mode.Initialize();
            this.Hide();
            Mode.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<string> BannedUserNames = new List<string>()
            {
                "server",
                "serveur",
                "serv",
                "admin",
                "adminf",
                "administrator",
                "administrateur",
                "moderator",
                "moderateur",
                "modo",
                "mod",
                "master",
                "pro"
            };
            if (BannedUserNames.Contains(textBox1.Text.ToLower()))
                textBox1.Text = "Toto";
            UserName = textBox1.Text;
            panel1.Enabled = UserName.Length > 0;
            if(Chat != null)
                Chat.tbUserName.Text = UserName;
            Net.SendMsg($"UpdateUserName|{UserName}");
        }
        private void btResendScores_Click(object sender, EventArgs e)
        {
            Net.SendScores(FileData.GetRaw());
            MessageBox.Show("Sent.");
        }

        private void btSaveName_Click(object sender, EventArgs e)
        {
            FileData.SaveData();
        }

        private void btScoreBoard_Click(object sender, EventArgs e)
        {
            Net.SendMsg("Scoreboard");
        }

        private void MakeScoreboard(string msg)
        {
            Invoke(new Action(() =>
            {
                if (Scoreboard == null)
                {
                    Scoreboard = new Scoreboard();
                    Scoreboard.FormClosed += delegate { Scoreboard = null; };
                    Scoreboard.UpdateData(msg);
                    Scoreboard.Show();
                }
                else
                    Scoreboard.UpdateData(msg);
            }));
        }

        private void ClientReceiveLoop(IAsyncResult result)
        {
            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(0, 0);
            byte[] message = socket.EndReceive(result, ref source);
            string data = Encoding.UTF32.GetString(message);
            string tag = "", msg = "";

            if (data.Split('|').Length == 1)
            {
                tag = data;
            }
            else
            {
                tag = data.Split('|')[0];
                msg = data.Split('|')[1];
            }

            switch (tag)
            {

                case "Scoreboard":
                    MakeScoreboard(msg);
                    break;

                case "pong":
                    Invoke(new Action(() => Icon = Properties.Resources.ServerUp));
                    if (Mode != null)
                        Invoke(new Action(() => Mode.Icon = Properties.Resources.ServerUp));
                    break;

                case "Scores":
                    new User(msg);
                    Console.WriteLine($"Received from [{source.Address}] :\n\t{msg.Replace("\n", "\n\t")}");
                    User.Write();
                    break;

                case "ChatboxMsg":
                    if (Chat != null)
                        Chat.Invoke(new Action(() =>
                        {
                            string[] infos = msg.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(',');
                            string strMsg = msg.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries)[1];
                            if (infos.Length == 2)
                            {
                                string strColor = infos[0];
                                string strName = infos[1];
                                Color color = Color.FromName(strColor);
                                Chat.ChatBox.SelectionColor = color;
                                Chat.ChatBox.SelectionFont = new Font(Chat.ChatBox.SelectionFont, FontStyle.Bold);
                                Chat.ChatBox.AppendText(strName + " : ");
                                Chat.ChatBox.AppendText(strMsg + "\n");
                                Chat.ChatBox.SelectionFont = new Font(Chat.ChatBox.SelectionFont, FontStyle.Regular);
                                Chat.ChatBox.SelectionColor = Color.FromArgb(200, 200, 200);
                            }
                            else
                            {
                                string strName = msg.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries)[0];
                                Chat.ChatBox.SelectionFont = new Font(Chat.ChatBox.SelectionFont, FontStyle.Bold);
                                Chat.ChatBox.AppendText(strName + " : ");
                                if(strName != UserName)
                                    Chat.ChatBox.SelectionFont = new Font(Chat.ChatBox.SelectionFont, FontStyle.Regular);
                                Chat.ChatBox.AppendText(strMsg + "\n");
                                if (strName == UserName)
                                    Chat.ChatBox.SelectionFont = new Font(Chat.ChatBox.SelectionFont, FontStyle.Regular);
                            }
                        }));
                    break;

                case "GetUsers":
                    string[] users = msg.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (users.Length == 0)
                        break;
                    if (Chat != null)
                    {
                        List<string> usersNames = new List<string>();
                        foreach (string user in users)
                            usersNames.Add(users[0].Split('-').Length > 1 ? user.Split('-')[0] : user);
                        Chat.Invoke(new Action(() =>
                        {
                            Chat.listUsers.Items.Clear();
                            Chat.listUsers.Items.AddRange(usersNames.ToArray());
                        }));
                    }
                    if(users[0].Split('-').Length > 1)
                        Mode?.GetUsers(users);
                    break;


                case "AttackP2P_AskDuel":
                    if (!(Mode is AttackWaitOpponent))break;
                    (Mode as AttackWaitOpponent).NetAskDuel(msg, source);
                    break;
                case "AttackP2P_ReplyDuel":
                    if (!(Mode is AttackWaitOpponent))break;
                    (Mode as AttackWaitOpponent).NetDuelAnswer(msg, source);
                    break;
                case "AttackP2P_GridInitialize":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_GridInitialize(msg);
                    break;
                case "AttackP2P_Ready":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).timer.Start();
                    break;
                case "AttackP2P_Cursor":
                    if (!(Mode is Attack))break;
                    (Mode as Attack).AttackP2P_Cursor(msg);
                    break;
                case "AttackP2P_Score":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_Score(msg);
                    break;
                case "AttackP2P_Click":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_Click(msg);
                    break;
                case "AttackP2P_NewBlock":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_NewBlock(msg);
                    break;
                case "AttackP2P_ComboMade":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_ComboMade(msg);
                    break;
                case "AttackP2P_AskBlock":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_AskBlock(msg);
                    break;
                case "AttackP2P_Gameover":
                    if (!(Mode is Attack)) break;
                    (Mode as Attack).AttackP2P_Gameover(msg);
                    break;
            }

            socket.BeginReceive(new AsyncCallback(ClientReceiveLoop), socket);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Net.SendMsg("Logout");
        }
    }
}
