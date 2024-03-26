using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    static public class Net
    {
        static public Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static public UdpClient UDP = new UdpClient(11000);
#if DEBUG
        static readonly IPAddress IP = IPAddress.Parse("172.16.1.172");
#else
        static readonly IPAddress IP = IPAddress.Parse("192.168.1.151");
#endif

        static public void SendMsg(string msg)
        {
            IPEndPoint endPoint = new IPEndPoint(IP, 11000);
            byte[] bytes = Encoding.UTF32.GetBytes(msg);
            sock.SendTo(bytes, endPoint);
        }
        static public void SendMsg(byte[] data)
        {
            IPEndPoint endPoint = new IPEndPoint(IP, 11000);
            sock.SendTo(data, endPoint);
        }
        static public void SendMsg(string msg, IPEndPoint source)
        {
            IPEndPoint ep = new IPEndPoint(source.Address, 11000);
            byte[] bytes = Encoding.UTF32.GetBytes(msg);
            sock.SendTo(bytes, ep);
        }

        static public void SendScores(string raw)
        {
            SendMsg($"Scores|{raw}");
        }
    }
}
