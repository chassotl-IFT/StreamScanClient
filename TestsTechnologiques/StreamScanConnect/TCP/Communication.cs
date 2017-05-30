using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Infos;

namespace StreamScanConnect
{
    internal class Communication
    {
        #region Attributes
        private StreamWriter writer;
        private NetworkStream networkStream;
        public event EventHandler onStop;
        private IFormatter formatter;

        #endregion

        internal Communication(NetworkStream stream)
        {
            writer = new StreamWriter(stream);
            formatter = new BinaryFormatter();
            this.networkStream = stream;
        }

        internal Object SendMsg(string message)
        {
            Object infos = null;
            try
            {
                if (message != "exit")
                {
                    Write(message);
                    infos = formatter.Deserialize(networkStream);
                }
                else { onStop(this, new EventArgs()); }
            }
            catch (Exception ex)
            {
                //Connexion au serveur perdue
                throw new Exception("Connexion au serveur perdue");
            }
            return infos;
        }

        internal void Write(string msg)
        {
            try
            {
                writer.AutoFlush = true;
                writer.WriteLine(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
