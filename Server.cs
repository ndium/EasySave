using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasySaveV2.Model;
using SimpleTCP;
using EasySaveV2.View;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Windows.Shapes;
using EasySaveV2.View_Model;

namespace EasySaveV2
{
    public class Server
    {
        Thread _thread;
        private Socket clientSocket;
        private List<Config> listConfigs;
        private List<Config> listConfigsToCopy;

        public Server()
        {
            _thread = new Thread(ServerTask);
            _thread.Start();
        }

        public void ServerTask()
        {
            // Créez une instance de Socket pour écouter les connexions
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Définissez l'adresse IP et le numéro de port sur lequel le serveur va écouter
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // localhost
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8000); // numéro de port arbitraire

            // Associez le Socket à l'adresse IP et au numéro de port
            serverSocket.Bind(ipEndPoint);

            // Démarrez le Socket en mode écoute
            serverSocket.Listen(10); // 10 est le nombre maximum de connexions en attente

            // Acceptez les connexions des clients
            clientSocket = serverSocket.Accept();

            // Envoyez un message au client
            string message = "Connected";
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(message);
            clientSocket.Send(messageBytes);


            //On envoit la liste de sauvegarde
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2\config.json";
            string temp = File.ReadAllText(path);

            //On alimente la liste de sauvegarde
            listConfigs = new List<Config>();
            
            listConfigs = JsonConvert.DeserializeObject<List<Config>>(temp);

            messageBytes = Encoding.ASCII.GetBytes(temp);
            clientSocket.Send(messageBytes);

            listenClient();
        }

        private void listenClient()
        {
            CopyViewModel copyViewModel = new CopyViewModel();

            while (true)
            {
                // Attendre de recevoir des données de la socket du client
                byte[] buffer = new byte[4096];
                int bytesRead = clientSocket.Receive(buffer);

                // Convertir les données reçues en une chaîne de caractères
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                switch (data)
                {
                    case "play":
                        // Attendre de recevoir l'indice de la sauvegarde à lancer
                        byte[] bufferPlay = new byte[4096];
                        int bytesReadPlay = clientSocket.Receive(buffer);

                        // Convertir les données reçues en une chaîne de caractères
                        string dataPlay = Encoding.ASCII.GetString(bufferPlay, 0, bytesReadPlay);

                        listConfigsToCopy = new List<Config>();
                        listConfigsToCopy = JsonConvert.DeserializeObject<List<Config>>(dataPlay);

                        //copyViewModel.GetCopyModel(listConfigsToCopy, null);
                        break;
                    case "pause":
                        break;
                    case "stop":
                        break;
                    case "refresh":
                        break;

                    default:
                        break;

                }
            }
        }
    }
}
