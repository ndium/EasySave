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
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Shapes;
using System.Windows.Threading;
using EasySaveV2.View_Model;
using System.Windows.Documents;

namespace EasySaveV2
{
    public class Server
    {
        Thread _thread;
        private Socket clientSocket;
        private List<Config> listConfigs;
        private List<Config> listConfigsToCopy;
        private SavesView savesView;
        private CopyViewModel copyViewModel;

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
            bool boolPause = false;

            while (true)
            {
                string resultat;
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
                        int bytesReadPlay = clientSocket.Receive(bufferPlay);

                        // Convertir les données reçues en une chaîne de caractères
                        string dataPlay = Encoding.ASCII.GetString(bufferPlay, 0, bytesReadPlay);

                        listConfigsToCopy = new List<Config>();
                        listConfigsToCopy = JsonConvert.DeserializeObject<List<Config>>(dataPlay);

                        if (Application.Current.Dispatcher.CheckAccess())
                        {
                            savesView = new SavesView();
                            savesView.ServerStartSave(listConfigsToCopy);
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                                {
                                    savesView = new SavesView();
                                    savesView.ServerStartSave(listConfigsToCopy);
                                }
                                    );
                        }

                        if (listConfigsToCopy.Count == 0)
                        {
                            // Envoyez un message au client
                            resultat = "Aucune sauvegarde selectionnee";
                        }
                        else
                        {
                            // Envoyez un message au client
                            resultat = "Sauvegarde lancee";

                        }
                        byte[] messageBytesPlay = System.Text.Encoding.ASCII.GetBytes(resultat);
                        clientSocket.Send(messageBytesPlay);
                        break;
                    case "pause":
                        // Attendre de recevoir l'indice de la sauvegarde à lancer
                        byte[] bufferPause = new byte[4096];
                        int bytesReadPause = clientSocket.Receive(bufferPause);

                        // Convertir les données reçues en une chaîne de caractères
                        string dataPause = Encoding.ASCII.GetString(bufferPause, 0, bytesReadPause);

                        copyViewModel = new CopyViewModel();
                        listConfigsToCopy = new List<Config>();
                        listConfigsToCopy = JsonConvert.DeserializeObject<List<Config>>(dataPause);

                        foreach (Config config in listConfigsToCopy)
                        {
                            copyViewModel.PauseThread(config.BackupName);
                        }

                        if (listConfigsToCopy.Count == 0)
                        {
                            // Envoyez un message au client
                            resultat = "Aucune sauvegarde selectionnee";
                        }
                        else
                        {
                            if (!boolPause)
                            {
                                // Envoyez un message au client
                                resultat = "Sauvegarde en pause";
                                boolPause = true;
                            }
                            else
                            {
                                // Envoyez un message au client
                                resultat = "Sauvegarde en cours";
                                boolPause = false;
                            }
                        }
                        byte[] messageBytesPause = System.Text.Encoding.ASCII.GetBytes(resultat);
                        clientSocket.Send(messageBytesPause);
                        break;
                    case "stop":
                        // Attendre de recevoir l'indice de la sauvegarde à lancer
                        byte[] bufferStop = new byte[4096];
                        int bytesReadStop = clientSocket.Receive(bufferStop);

                        // Convertir les données reçues en une chaîne de caractères
                        string dataStop = Encoding.ASCII.GetString(bufferStop, 0, bytesReadStop);

                        //copyViewModel = new CopyViewModel();
                        listConfigsToCopy = new List<Config>();
                        listConfigsToCopy = JsonConvert.DeserializeObject<List<Config>>(dataStop);

                        foreach (Config config in listConfigsToCopy)
                        {
                            //On arrete les threads de la list
                            copyViewModel.StopThread(config.BackupName);
                        }

                        if (listConfigsToCopy.Count == 0)
                        {
                            // Envoyez un message au client
                            resultat = "Aucune sauvegarde selectionnee";
                        }
                        else
                        {
                            // Envoyez un message au client
                            resultat = "Sauvegarde arrete";
                            
                        }
                        byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(resultat);
                        clientSocket.Send(messageBytes);
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
