using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace EasySaveV2.Model
{
    public class CopyModel
    {

        private int _progress { get; set; }
       
        



        public void FullCopy(Config config, object sender)
        {
            var localWorker = sender as BackgroundWorker;
            localWorker.ReportProgress(0, string.Format("0"));
            // Effectuer la sauvegarde si possible
            if (!CheckBusinessApp())
                throw new Exception("Impossible de sauvegarder car une des applications enregistrées est en cours d'exécution");


            string sourceFile = config.SourceDirectory;
            string targetFile = config.TargetDirectory;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (sourceFile != null)
            {
                var dir = new DirectoryInfo(sourceFile);

                if (!dir.Exists)
                {


                    var fileinfo = new FileInfo(sourceFile);
                    string filename = Path.GetFileName(sourceFile);
                    var fileSize = fileinfo.Length;
                    targetFile = Path.Combine(targetFile, filename);
                    if (!fileinfo.Exists)
                        throw new FileNotFoundException($"{sourceFile} does not exist");
                    using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var target = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                        {
                            // Allocation d'un buffer de lecture de 4 Ko
                            var buffer = new byte[4096];
                            int bytesRead;
                            int totalBytes = 0;

                            // Boucle de lecture et d'écriture
                            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                target.Write(buffer, 0, bytesRead);
                                totalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((totalBytes / (double)fileSize) * 100);
                                localWorker.ReportProgress(_progress, string.Format($"{_progress}%"));
                            }
                        }
                    }
                    watch.Stop();
                    double timeElapsed = watch.Elapsed.TotalSeconds;
                    var logJsonModel = new LogJsonModel();
                    logJsonModel.SaveLog(fileSize, timeElapsed, config);

                }
                else
                {
                    try
                    {
                        
                        DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFile);
                        long totalSize = GetDirectorySize(sourceFolderInfo);
                        long totalBytes = 0;

                         CopyDirectory(sourceFolderInfo, targetFile, totalSize, totalBytes);

                        /*foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
                        {
                            var source = fileInfo.FullName;
                            var target = Path.Combine(targetFile, fileInfo.Name);
                            await EncryptionFile( source, target);
                        }

                        foreach (DirectoryInfo subDir in sourceFolderInfo.GetDirectories())
                        {
                            string newDestinationDir = Path.Combine(targetFile, subDir.Name);
                            await EncryptionFile(sourceFile, newDestinationDir);
                        }*/

                        EncryptionRecursiveFile(sourceFolderInfo, sourceFile, targetFile);

                        watch.Stop();
                        double timeElapsed = watch.Elapsed.TotalSeconds;
                        LogJsonModel logJsonModel = new LogJsonModel();
                        logJsonModel.SaveLog(totalSize, timeElapsed, config);
                    }
                    catch (Exception e)
                    {
                        
                    }
                }


            }

            async Task EncryptionRecursiveFile(DirectoryInfo sourceFolderInfo,string sourceFile, string targetFile)
            {
                foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
                {
                    var source = fileInfo.FullName;
                    var target = Path.Combine(targetFile, fileInfo.Name);
                    await EncryptionFile(source, target);
                }

                foreach (DirectoryInfo subDir in sourceFolderInfo.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(targetFile, subDir.Name);
                    string newSource = Path.Combine(sourceFile, subDir.Name);
                    await EncryptionRecursiveFile(subDir, newSource, newDestinationDir);
                }
            }

            async Task CopyDirectory(DirectoryInfo sourceDirectoryInfo, string targetFolder, long totalSize, long totalBytes)
            {
                var localWorker = sender as BackgroundWorker;
                localWorker.ReportProgress(0, string.Format("0"));
                if (!CheckBusinessApp())
                {
                    throw new Exception("Impossible de sauvegarder car une des applications enregistrées est en cours d'exécution");
                }

                // Création du dossier cible s'il n'existe pas
                Directory.CreateDirectory(targetFolder);


                // Boucle de copie des fichiers
                foreach (var fileInfo in sourceDirectoryInfo.GetFiles())
                {

                    var sourceFile = fileInfo.FullName;
                    var targetFile = Path.Combine(targetFolder, fileInfo.Name);

                    // Copie du fichier
                    using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var target = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                        {
                            var buffer = new byte[4096];
                            int bytesRead;

                            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                target.Write(buffer, 0, bytesRead);
                                totalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((totalBytes / (double)totalSize) * 100);
                                localWorker.ReportProgress(_progress, string.Format($"{_progress}%"));
                            }

                        }
                    }
                }




                foreach (DirectoryInfo subDir in sourceDirectoryInfo.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(targetFolder, subDir.Name);
                    CopyDirectory(subDir, newDestinationDir, totalSize, totalBytes);
                }
            }
        }
        private static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            // Récuperation de la taille des fichiers dans le dossier
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                size += fileInfo.Length;
            }

            // Récuperation de la taille des sous-dossiers récursivement
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                size += GetDirectorySize(subDirectoryInfo);
            }

            return size;
        }
        public async Task DifferentialCopy(Config config,  object sender)
        {
            if (CheckBusinessApp())
            {
                string sourceFolder = config.SourceDirectory;
                string targetFolder = config.TargetDirectory;
                var sourceFolderInfo = new DirectoryInfo(sourceFolder);
                long totalSize =  GetDirectorySize(sourceFolderInfo);
                foreach (var fileInfo in new DirectoryInfo(sourceFolder).GetFiles())
                {
                    var sourceFile = fileInfo.FullName;
                    var targetFile = Path.Combine(targetFolder, fileInfo.Name);

                    // Vérification date dernière modification du fichier
                    var sourceLastWriteTime = File.GetLastWriteTime(sourceFile);
                    if (File.Exists(targetFile))
                    {
                        var targetLastWriteTime = File.GetLastWriteTime(targetFile);
                        if (sourceLastWriteTime <= targetLastWriteTime)
                        {
                            continue;
                        }
                    }

                    using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var target = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                        {
                            var buffer = new byte[4096];
                            int bytesRead;
                            int totalBytes = 0;


                            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                target.Write(buffer, 0, bytesRead);
                                totalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((totalBytes / (double)totalSize) * 100);

                            }

                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Impossible de sauvegarder car une des applications enregistrées est en cours d'exécution");
            }
        }

        public bool CheckBusinessApp()
        {
            // Charger les applications à partir du fichier JSON

            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";
            string file = Path.Combine(appPath, "applications.json");
            if (!File.Exists(file))
            {
                return true;
            }
            string json = File.ReadAllText(file);
            var applications = JsonConvert.DeserializeObject<string[]>(json);

            // Vérifier si les applications sont ouvertes
            bool canSave = true;
            foreach (string application in applications)
            {
                Process[] processList = Process.GetProcessesByName(application);
                if (processList.Length > 0)
                {
                    Console.WriteLine("L'application " + application + " est en cours d'exécution et bloque la sauvegarde");
                    canSave = false;
                    break;
                }
            }
            return canSave;
        }

        //Méthode qui gère le chiffrement de fichier un à un
        public async Task EncryptionFile(string source, string destination)
        {
            FileEncryptModel fileEncryptModel = new FileEncryptModel();

            foreach (Extension extension in fileEncryptModel.GetList())
            {
                if (extension.Name == Path.GetExtension(source))
                {
                    Process processCryptosoft = new Process();
                    processCryptosoft.StartInfo.FileName = @"Model\CryptoSoft.exe";
                    //processCryptosoft.StartInfo.Arguments = source + " " + destination;
                    processCryptosoft.StartInfo.ArgumentList.Add(source);
                    processCryptosoft.StartInfo.ArgumentList.Add(destination);
                    processCryptosoft.Start();
                }
            }
            
        }
    }
}



