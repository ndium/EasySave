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
using System.Windows;

namespace EasySaveV2.Model
{
    public class CopyModel
    {
        private string workName { get; set; }
        private int _progress { get; set; }
        private long TotalBytes { get; set; }



        public async void FullCopy(Config config, object sender)
        {
            workName = config.BackupName;
            TotalBytes = 0;
            int oldProgress = 0;
            var localWorker = sender as BackgroundWorker;
            localWorker.ReportProgress(0, string.Format($"{config.BackupName}"));
            // Effectuer la sauvegarde si possible
            if (!CheckBusinessApp())
            {
                throw new Exception("Impossible de sauvegarder car une des applications enregistrées est en cours d'exécution");

            }


            string sourceFile = config.SourceDirectory;
            string targetFile = config.TargetDirectory;
            var logJsonModel = new LogJsonModel();
            var statelog = new Statelog();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (sourceFile != null)
            {
                var dir = new DirectoryInfo(sourceFile);
                //Passe dans la boucle si il s'agit d'un simple fichier
                if (!dir.Exists)
                {

                    bool state = true;
                    var fileinfo = new FileInfo(sourceFile);
                    var fileSize = fileinfo.Length;
                    if (!fileinfo.Exists)
                        throw new FileNotFoundException($"{sourceFile} does not exist");
                    using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var target = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                        {
                            // Allocation d'un buffer de lecture de 4 Ko
                            var buffer = new byte[4096];
                            int bytesRead;

                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            double byteTimeElapsed = 0;
                            // Boucle de lecture et d'écriture
                            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                            {

                                target.Write(buffer, 0, bytesRead);
                                TotalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((TotalBytes / (double)fileSize) * 100);
                                if (_progress != 100)
                                {
                                    if (oldProgress != _progress)
                                    {
                                        localWorker.ReportProgress(_progress, string.Format($"{config.BackupName}"));
                                        oldProgress = _progress;
                                    }
                                    statelog.SaveLog(fileSize, byteTimeElapsed, fileSize, 1, config, state);
                                }
                            }

                        }
                    }

                     EncryptionRecursiveFile(null, sourceFile, targetFile);
                    watch.Stop();
                    double timeElapsed = watch.Elapsed.TotalSeconds;

                    logJsonModel.SaveLog(fileSize, timeElapsed, config);
                    localWorker.ReportProgress(100, string.Format($"{config.BackupName}"));


                }
                //Sinon passe dans la boucle pour copier les dossiers
                else
                {
                    try
                    {

                        DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFile);
                        long totalSize = GetDirectorySize(sourceFolderInfo);


                        await CopyDirectory(sourceFolderInfo, targetFile, totalSize);


                         EncryptionRecursiveFile(sourceFolderInfo, sourceFile, targetFile);

                        watch.Stop();
                        double timeElapsed = watch.Elapsed.TotalSeconds;
                        logJsonModel.SaveLog(totalSize, timeElapsed, config);
                        statelog.SaveLog(totalSize, timeElapsed, 1, 1, config, true);
                        localWorker.ReportProgress(100, string.Format($"{config.BackupName}"));

                    }
                    catch (Exception e)
                    {

                    }

                }


            }

            void EncryptionRecursiveFile(DirectoryInfo? sourceFolderInfo, string sourceFile, string targetFile)
            {
                if (sourceFolderInfo != null)
                {
                    foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
                    {
                        var source = fileInfo.FullName;
                        var target = Path.Combine(targetFile, fileInfo.Name);
                         EncryptionFile(source, target);
                    }

                    foreach (DirectoryInfo subDir in sourceFolderInfo.GetDirectories())
                    {
                        string newDestinationDir = Path.Combine(targetFile, subDir.Name);
                        string newSource = Path.Combine(sourceFile, subDir.Name);
                         EncryptionRecursiveFile(subDir, newSource, newDestinationDir);
                    }
                }
                else
                {
                     EncryptionFile(sourceFile, targetFile);
                }
            }

            async Task CopyDirectory(DirectoryInfo sourceDirectoryInfo, string targetFolder, long totalSize)
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
                                TotalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((TotalBytes / (double)totalSize) * 100);

                                if (_progress != 100)
                                {
                                    if (oldProgress != _progress)
                                    {
                                        localWorker.ReportProgress(_progress, string.Format($"{config.BackupName}"));
                                        oldProgress = _progress;
                                    }
                                }


                            }

                        }
                    }

                }






                foreach (DirectoryInfo subDir in sourceDirectoryInfo.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(targetFolder, subDir.Name);
                    CopyDirectory(subDir, newDestinationDir, totalSize);
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
        public async Task DifferentialCopy(Config config, object sender)
        {
            int TotalBytes = 0;
            int oldProgress = 0;
            var localWorker = sender as BackgroundWorker;
            if (CheckBusinessApp())
            {
                string sourceFolder = config.SourceDirectory;
                string targetFolder = config.TargetDirectory;
                var sourceFolderInfo = new DirectoryInfo(sourceFolder);
                long totalSize = GetDirectorySize(sourceFolderInfo);
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


                            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                target.Write(buffer, 0, bytesRead);
                                TotalBytes += bytesRead;

                                // Calcul de la progression
                                _progress = (int)((TotalBytes / (double)totalSize) * 100);
                                if (_progress != 100)
                                {
                                    if (oldProgress != _progress)
                                    {
                                        localWorker.ReportProgress(_progress, string.Format($"{config.BackupName}"));
                                        oldProgress = _progress;
                                    }
                                }

                            }

                        }
                    }
                }
            }
            else
            {
                throw new Exception("Impossible de sauvegarder car une des applications enregistrées est en cours d'exécution");
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
            List<BusinessApp> applications = JsonConvert.DeserializeObject<List<BusinessApp>>(json);

            // Vérifier si les applications sont ouvertes
            bool canSave = true;
            foreach (var application in applications)
            {
                Process[] processList = Process.GetProcessesByName(application.AppName);
                if (processList.Length > 0)
                {
                    canSave = false;
                    break;
                }
            }
            return canSave;
        }

        //Méthode qui gère le chiffrement de fichier un à un
        public void EncryptionFile(string source, string destination)
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



