using SharedClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerScoreStoring
{
    partial class Program
    {
        static Dictionary<IPAddress, IPAddress> AttackP2PReady = new Dictionary<IPAddress, IPAddress>();
        static Dictionary<(IPAddress IP, int Score), (IPAddress IP, int Score)> AttackP2PGameover = new Dictionary<(IPAddress IP, int Score), (IPAddress IP, int Score)>();

        static void Login(string msg, IPEndPoint source)
        {
            if (!Clients.ContainsKey(msg) && !Clients.ContainsValue(source))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[+] Login [{msg}:{source}]");
                Console.ResetColor();
                Clients[msg] = source;
                if (!ClientsTimeout.ContainsKey(source))
                {
                    ClientsTimeout[source] = new Stopwatch();
                    ClientsTimeout[source].Start();
                }
                GetUsers(msg, source);
            }
        }
        static void LogMeOut(string msg, IPEndPoint source, string key)
        {
            LockClients = true;
            Clients.Remove(key);
            if (ClientsTimeout.ContainsKey(source))
                ClientsTimeout.Remove(source);
            GetUsers(msg, source);

            if (AttackP2PReady.ContainsKey(source.Address))
            {
                AttackP2PReady.Remove(source.Address);
            }
            else if (AttackP2PReady.ContainsValue(source.Address))
            {
                var found = AttackP2PReady.Keys.FirstOrDefault(k => k.ToString().CompareTo(source.Address) == 0);
                AttackP2PReady.Remove(found);
            }

            var keyfound = AttackP2PGameover.Keys.FirstOrDefault(k => k.IP.ToString().CompareTo(source.Address.ToString()) == 0);
            if (!keyfound.Equals(default((IPAddress IP, int Score))))
            {
                AttackP2PGameover.Remove(keyfound);
            }
            else
            {
                var valuefound = AttackP2PGameover.Values.FirstOrDefault(k => k.IP.ToString().CompareTo(source.Address.ToString()) == 0);
                if (!valuefound.Equals(default((IPAddress IP, int Score))))
                {
                    AttackP2PGameover.Remove(valuefound);
                }
            }

            LockClients = false;
        }
        static void TimeoutLogout(string msg, IPEndPoint source)
        {
            if (Clients.ContainsValue(source))
            {
                string key = GetKey(source);
                Console.WriteLine($"[-] Timeout : Logout [{key}:{source}]");
                LogMeOut(msg, source, key);
            }
        }
        static void Logout(string msg, IPEndPoint source)
        {
            if (Clients.ContainsValue(source))
            {
                string key = GetKey(source);
                string A = Clients.First().Value.ToString();
                string B = source.ToString();
                if (string.IsNullOrWhiteSpace(key))
                    return;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[-] Logout [{key}:{source}]");
                Console.ResetColor();
                LogMeOut(msg, source, key);
            }
        }
        static void GetUsers(string msg, IPEndPoint source)
        {
            string clients = "";
            foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                if (client.Value.ToString().CompareTo(source.ToString()) != 0)
                    clients += client.Key + (msg == "Full" ? $"-{client.Value}," : ",");
            /*foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                if (client.Value != source)*/
            Net.SendMsg($"GetUsers|{clients}", source);
        }
        static void Scoreboard(string msg, IPEndPoint source)
        {
            Console.WriteLine($"Scoreboard asked by [{source}]");
            Net.SendMsg($"Scoreboard|{User.ToString()}", source);
        }
        static void Ping(string msg, IPEndPoint source)
        {
            Net.SendMsg("pong", source);
            if (ClientsTimeout.ContainsKey(source))
                ClientsTimeout[source].Restart();
            if (!Clients.ContainsValue(source))
                Login(msg, source);
        }
        static void Scores(string msg, IPEndPoint source)
        {
            new User(msg);
            Console.WriteLine($"Received from [{source}] :\n\t{msg.Replace("\n", "\n\t")}");
            User.Write();
        }
        static void ChatboxMsg(string data)
        {
            string datamsg = data.Split('|')[1];
            if (datamsg.Split(',').Length == 2)
                datamsg = datamsg.Split(',')[1];
            string username = datamsg.Split(new[] { " : " }, StringSplitOptions.None)[0];
            string msg = datamsg.Split(new[] { " : " }, StringSplitOptions.None)[1];
            string source = username == "Server" ? "" : Clients[username].ToString();
            string result = $"Chat > {username} ({source}) : {msg}";
            ChatBoxHistory += result + "\n";

            if (ShowChat)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(result);
                Console.ResetColor();
            }

            foreach (KeyValuePair<string, IPEndPoint> client in Clients)
                Net.SendMsg(data, client.Value);
        }
        static void UpdateUserName(string msg, IPEndPoint source)
        {
            if (Clients.ContainsValue(source))
            {
                string key = GetKey(source);
                string OldUserName = key;
                Clients.Remove(key);
                Clients[msg] = source;
                GetUsers(msg, source);
                Console.WriteLine($"UserName changed at [{source}] from '{OldUserName}' to '{msg}'");
            }
        }
        static void AttackP2P_Ready(string msg, IPEndPoint source)
        {
            var found = AttackP2PReady.Keys.FirstOrDefault(key => key.ToString().CompareTo(msg) == 0);
            if (found != null)
            {
                Net.SendMsg("AttackP2P_Ready", source);
                Net.SendMsg("AttackP2P_Ready", new IPEndPoint(IPAddress.Parse(msg), 11000));
                AttackP2PReady.Remove(found);
                Console.WriteLine($"A duel begins between {GetKey(msg)} [{msg}] and {GetKey(source.Address.ToString())} [{source.Address.ToString()}]");
            }
            else
            {
                AttackP2PReady[source.Address] = IPAddress.Parse(msg);
            }
        }
        static void AttackP2P_Gameover(string msg, IPEndPoint source)
        {
            string[] data = msg.Split(',');
            var found = AttackP2PGameover.Keys.FirstOrDefault(key => key.IP.ToString().CompareTo(data[1]) == 0);
            if (found.IP != null)
            {
                int winner = found.Score > int.Parse(data[0]) ? 0 : (found.Score < int.Parse(data[0]) ? 1 : 2);
                Net.SendMsg($"AttackP2P_Gameover|{(winner == 1 ? "W" : (winner == 0 ? "L" : "D"))},{found.Score}", source);
                Net.SendMsg($"AttackP2P_Gameover|{(winner == 1 ? "L" : (winner == 0 ? "W" : "D"))}", new IPEndPoint(IPAddress.Parse(data[1]), 11000));
                AttackP2PGameover.Remove(found);
                Console.WriteLine($"The duel between {GetKey(data[1])} [{data[1]}] and {GetKey(source)} [{source.Address.ToString()}] ends.\n  → {(winner == 1 ? GetKey(source) + " wins !" : (winner == 0 ? GetKey(data[1]) + " wins !" : "Draw!"))}");
            }
            else
            {
                AttackP2PGameover[(source.Address, int.Parse(data[0]))] = (IPAddress.Parse(data[1]), int.Parse(data[0]));
            }
        }
    }
}
