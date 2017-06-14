using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamScanConnect
{
    public class ClientTCP
    {
        private TcpClient client;
        private Communication com;

        public void InitClient(string serverAddr)
        {
            this.InitClient(serverAddr, 1500);
        }
        public void InitClient(string serverAddr, int port)
        {
            try
            {
                client = new TcpClient();
                client.ReceiveTimeout = 5000;
                client.Connect(serverAddr, port);
                Console.WriteLine("Connecté !");
                NetworkStream stream = client.GetStream();
                com = new Communication(stream);

                com.onStop += com_onStop;
            }
            catch (Exception)
            {
            }
        }

        internal void com_onStop(object sender, EventArgs e)
        {
            client.Close();
        }

        public Object SendMessage(string message)
        {
            return com.SendMsg(message);
        }

        public static bool CheckStatus(string ip, int port)
        {
            bool status = false;
            try
            {
                TcpClient client = new TcpClient(ip, port);
                status = true;
                client.Close();
            }
            catch (Exception)
            {
            }
            return status;
        }
    }
}
