using SharedClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerScoreStoring
{
    partial class Program
    {
        static Dictionary<string, IPEndPoint> Clients = new Dictionary<string, IPEndPoint>();
        static Dictionary<IPEndPoint, Stopwatch> ClientsTimeout = new Dictionary<IPEndPoint, Stopwatch>();
        static short TimeoutSeconds = 10;
        static System.Timers.Timer ManageClientsTimeout = new System.Timers.Timer();
        static bool LockClients = false;
        static bool m_ShowChat = false;
        static bool ShowChat { get => m_ShowChat; set { m_ShowChat = value; UpdateTitle(); } }
        static string ChatBoxHistory = "";
        static bool m_FullLogs = false;
        static bool FullLogs { get => m_FullLogs; set { m_FullLogs = value; UpdateTitle(); } }
        static string ScoreboardColumns
        {
            get
            {
                return string.Format("{0,-12}  {1, -10}  {2, -10}  {3, -10}  {4, -10}  {5, -10}  {6, -10}  {7, -10}  {8, -10}", "UserName", "Zen", "Bombs", "Surv.Min", "Surv.Max", "Hits", "Timer", "SwitchHits", "Glue") + "\n";
            }
        }

        static void Main(string[] args)
        {
            User.Read();
            Console.WriteLine($"Scoreboard :\n{ScoreboardColumns}{User.ToString()}\n");
            UpdateTitle();

            Net.UDP.BeginReceive(new AsyncCallback(HostReceiveLoop), Net.UDP);

            ManageClientsTimeout.Interval = TimeoutSeconds;
            ManageClientsTimeout.Elapsed += ManageClientsTimeout_Elapsed;
            ManageClientsTimeout.Start();

            while (true)
            {
                Inputs();
            }
        }

        private static void UpdateTitle()
        {
            Console.Title = $"Server | Chat [{(ShowChat ? "ON" : "OFF")}] | Logs [{(FullLogs ? "ON" : "OFF")}]";
        }

        static void ManageClientsTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (LockClients)
                return;

            List<IPEndPoint> ListOfClientsToTimeout = new List<IPEndPoint>();
            foreach (var client in ClientsTimeout)
            {
                if (client.Value.Elapsed.Seconds > TimeoutSeconds)
                {
                    ListOfClientsToTimeout.Add(client.Key);
                }
            }
            foreach (var clientToTimeout in ListOfClientsToTimeout)
            {
                if (Clients.ContainsValue(clientToTimeout))
                {
                    TimeoutLogout(GetKey(clientToTimeout), clientToTimeout);
                }
            }
        }
        static string GetKey(IPEndPoint source)
        {
            return Clients.Keys.FirstOrDefault(x => Clients[x].Address.ToString().CompareTo(source.Address.ToString()) == 0);
        }
        static string GetKey(string sourceAddress)
        {
            return Clients.Keys.FirstOrDefault(x => Clients[x].Address.ToString().CompareTo(sourceAddress) == 0);
        }

        static void HostReceiveLoop(IAsyncResult result)
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

            if (FullLogs)
            {
                Console.WriteLine($"> Received from [{source.Address}] {data.Length} bytes :");
                Console.WriteLine("\t" + data);
            }

            switch (tag)
            {

                case "Scoreboard":
                    Scoreboard(msg, source);
                    break;

                case "ping":
                    Ping(msg, source);
                    break;

                case "Scores":
                    Scores(msg, source);
                    break;

                case "ChatboxMsg":
                    ChatboxMsg(data);
                    break;

                case "GetUsers":
                    GetUsers(msg, source);
                    break;

                case "Login":
                    Login(msg, source);
                    break;

                case "Logout":
                    Logout(msg, source);
                    break;

                case "UpdateUserName":
                    UpdateUserName(msg, source);
                    break;

                case "DebugInfo":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(data);
                    Console.ResetColor();
                    break;

                case "AttackP2P_Ready":
                    AttackP2P_Ready(msg, source);
                    break;

                case "AttackP2P_Gameover":
                    AttackP2P_Gameover(msg, source);
                    break;

            }

            socket.BeginReceive(new AsyncCallback(HostReceiveLoop), socket);
        }

        private static void Inputs()
        {
            string input = Console.ReadLine();
            string[] words = input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return;
            string cmd = words[0];
            string arg = words.Length > 1 ? string.Join(" ",words.Skip(1)) : null;

            Commands parsedcmd;
            if (!Enum.TryParse(cmd, true, out parsedcmd))
            {
                Console.WriteLine("Command not recognized. Try 'help' to get the commands list.");
                return;
            }

            switch (parsedcmd)
            {

                case Commands.Help:
                    string output = "[" + string.Join("]\n\t[", Enum.GetNames(typeof(Commands))) + "]";
                    Console.WriteLine($"\t{output}\n");
                    break;

                case Commands.Scoreboard:
                    Console.WriteLine($"Scoreboard :\n{ScoreboardColumns}{User.ToString()}\n");
                    break;

                case Commands.Clear:
                    Console.Clear();
                    break;

                case Commands.ForceLogout:// arg : UserName
                    if (arg == null)
                        break;
                    Console.WriteLine($"Force Logout on '{arg}' ({Clients[arg]})");
                    if (Clients.ContainsKey(arg))
                        Logout(arg, Clients[arg]);
                    break;

                case Commands.Chat:
                    if (arg != null)
                    {
                        ChatboxMsg($"ChatboxMsg|Orange,Server : {arg}");
                    }
                    else
                    {
                        Console.WriteLine("> Write message to diffuse :");
                        string msg = Console.ReadLine();
                        ChatboxMsg($"ChatboxMsg|Orange,Server : {msg}");
                    }
                    break;

                case Commands.ChatOn:
                    ShowChat = true;
                    Console.WriteLine("The Chat is [ON]");
                    break;

                case Commands.ChatOff:
                    ShowChat = false;
                    Console.WriteLine("The Chat is [OFF]");
                    break;

                case Commands.ChatH:// ChatBox History
                    NotepadHelper.ShowMessage(ChatBoxHistory, "Chatbox History");
                    break;

                case Commands.LogsOn:
                    FullLogs = true;
                    break;

                case Commands.LogsOff:
                    FullLogs = false;
                    break;

                case Commands.SetScore:
                    string[] fullargs = arg.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if(fullargs.Length == 3)
                    {
                        long val = 0;
                        if (!long.TryParse(fullargs[2], out val))
                            break;
                        if(!User.Users.ContainsKey(fullargs[0]))
                            break;
                        switch (fullargs[1])
                        {
                            case "0": User.Users[fullargs[0]].ZenScore = val; break;
                            case "1": User.Users[fullargs[0]].BombsScore = val; break;
                            case "2": User.Users[fullargs[0]].SurvivalMiniScore = val; break;
                            case "3": User.Users[fullargs[0]].SurvivalMaxScore = val; break;
                            case "4": User.Users[fullargs[0]].HitsScore = val; break;
                            case "5": User.Users[fullargs[0]].TimerScore = val; break;
                            case "6": User.Users[fullargs[0]].SwitchHitsScore = val; break;
                            case "7": User.Users[fullargs[0]].GlueScore = val; break;
                        }
                        User.Write();
                        Console.WriteLine($"Score mod done to user [{fullargs[0]}].");
                    }
                    break;

                case Commands.ResetModeScore:
                    if (string.IsNullOrWhiteSpace(arg))
                        break;
                    int modenumb = 0;
                    if (int.TryParse(arg, out modenumb) == false)
                        break;
                    if (modenumb < 0 || modenumb > 7) // --------------------------------- NUMBER OF MODES HERE
                        break;
                    Console.WriteLine("/!\\ Are you sure to proceed (Y/N)? /!\\");
                    if(Console.ReadKey(false).Key != ConsoleKey.Y)
                    {
                        Console.WriteLine("Action Cancelled.");
                        break;
                    }
                    foreach (var user in User.Users)
                    {
                        long val = 0;
                        switch (modenumb)
                        {
                            case 0: user.Value.ZenScore = val; break;
                            case 1: user.Value.BombsScore = val; break;
                            case 2: user.Value.SurvivalMiniScore = val; break;
                            case 3: user.Value.SurvivalMaxScore = val; break;
                            case 4: user.Value.HitsScore = val; break;
                            case 5: user.Value.TimerScore = val; break;
                            case 6: user.Value.SwitchHitsScore = val; break;
                            case 7: user.Value.GlueScore = val; break;
                        }
                    }
                    User.Write();
                    Console.WriteLine($"Mode Scores successfuly resetted.");
                    break;

            }
        }

        enum Commands { Help, Scoreboard, Clear, ForceLogout, Chat, ChatOn, ChatOff, ChatH, LogsOn, LogsOff, SetScore, ResetModeScore }
    }
}
