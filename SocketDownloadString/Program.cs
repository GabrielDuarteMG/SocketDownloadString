using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace SocketDownloadString
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Digite a URL(http://): "); string x = Console.ReadLine();
            Console.WriteLine(ConnectionsParameters(x, 80, 2000));
            Console.ReadKey();
        }
        private static Socket FinalConection(string href, int x)
        {
            Socket result = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(href);
            IPAddress[] addressList = hostEntry.AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                IPAddress address = addressList[i];
                IPEndPoint iPEndPoint = new IPEndPoint(address, x);
                if (!(iPEndPoint.AddressFamily.ToString() != "InterNetwork"))
                {
                    Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(iPEndPoint);
                    if (socket.Connected)
                    {
                        result = socket;
                        return result;
                    }
                }
            }
            return result;
        }
        private static string IntercessionConection(string href, int x, string string_22, int y)
        {
            string s = string.Concat(new string[]
             {
                "GET ",
                string_22,
                " HTTP/1.1\r\nHost: ",
                href,
                "\r\nConnection: Close\r\n\r\n"
             });
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] array = new byte[256];
            Socket socket = FinalConection(href, x);
            if (socket == null)
            {
                return "Falha na conexao";
            }
            socket.ReceiveTimeout = y;
            socket.Send(bytes, bytes.Length, SocketFlags.None);
            string text = "";
            int num;
            do
            {
                num = socket.Receive(array, array.Length, SocketFlags.None);
                text += Encoding.ASCII.GetString(array, 0, num);
            }
            while (num > 0);
            return text;
        }
        private static string ConnectionsParameters(string href, int x = 80, int y = 2000)
        {
            href = href.Replace("http://", "");
            href = href.Replace("https://", "");
            string minisplit = href.Split(new char[]
            {
                '/'
            })[0];
            string textsplit = "/" + string.Join("/", href.Split(new char[]
            {
                '/'
            }).Skip(1).ToArray<string>());
            string text = IntercessionConection(minisplit, x, textsplit, y);
            List<string> list = text.Split(new string[]
            {
                "\r\n",
                "\n"
            }, StringSplitOptions.None).ToList<string>();
            List<string> list2 = new List<string>();
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(list[i]))
                {
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        list2.Add(list[i]);
                    }
                }
            }
            return string.Join(Environment.NewLine, list2);
        }

    }
}
