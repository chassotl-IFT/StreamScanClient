using Infos;
using StreamScanCommon.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamScanConnect
{
    public class ClientWCF
    {
        #region Properties
        public static int defaultPort = 11252;
        public static string defaultServiceName = "StreamScan";
        IStreamScanInfos client = null;
        #endregion

        /// <summary>
        /// [Web service] Initialise le client avec le port 1500 et le nom de service "StreamScan".
        /// </summary>
        /// <param name="ip">L'adresse IP du serveur</param>
        public void InitClient(string ip)
        {
            this.InitClient(ip, defaultPort, defaultServiceName);
        }

        /// <summary>
        /// [Web service] Initialise le client avec le nom de service "StreamScan".
        /// </summary>
        /// <param name="ip">L'adresse IP du serveur</param>
        /// <param name="port">Le port utilisé par la communication</param>
        public void InitClient(string ip, int port)
        {
            this.InitClient(ip, port, defaultServiceName);
        }

        /// <summary>
        /// [Web service] Initialise le client.
        /// </summary>
        /// <param name="ip">L'adresse IP du serveur</param>
        /// <param name="port">Le port utilisé par la communication</param>
        /// <param name="serviceName">Le nom du service utilisé pour récupérer les infos</param>
        public void InitClient(string ip, int port, string serviceName)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress($"http://{ip}:{port}/{serviceName}");
            ChannelFactory<IStreamScanInfos> myChannelFactory = new ChannelFactory<IStreamScanInfos>(myBinding, myEndpoint);

            client = myChannelFactory.CreateChannel();
        }

        public Object SendMessage(string message)
        {
            try
            {
                Type clientType = client.GetType();

                switch (message)
                {
                    case "help":
                        string retour = "Méthodes disponibles :<br/>";
                        foreach (MethodInfo met in clientType.GetMethods())
                        {
                            retour += "  " + met.Name + "<br/>";
                        }
                        return retour;
                    case "exit":
                        ((IClientChannel)client).Close();
                        return "La connexion à été fermée.";
                    default:
                        try { 
                        MethodInfo method = clientType.GetMethod(message);
                        var infos = method.Invoke(client, null);
                        return infos;
                        }
                        catch(Exception)
                        {
                            return "La connexion au serveur a été perdue";
                        }
                }

            }
            catch (NullReferenceException)
            {
                return "Méthode inconnue. Tapez \"help\" pour obtenir la liste des méthodes disponibles.";
            }
            catch (Exception ex)
            {
                //Connexion au serveur perdue
                return ex.Message;
            }
        }

        public static bool CheckStatus(string url)
        {
            bool status = false;
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)myRequest.GetResponse();
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}