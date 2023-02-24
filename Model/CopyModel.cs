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
using EasySaveV2.View;
using System.Windows.Automation.Peers;
using System.Dynamic;
using EasySaveV2.View_Model;

namespace EasySaveV2.Model
{
    public class CopyModel
    {
        public string workName { get; set; }
        private int _progress { get; set; }
        private long TotalBytes { get; set; }
        public static List<CopyModel> copyModels = new List<CopyModel>();
        private bool isPaused { get; set; }
        private bool isStopped { get; set; }

        private FiltersModel _filtersModel;

        public CopyModel()
        {
            _filtersModel = new FiltersModel();
        }

        public void FullCopy(Config config, object sender)
        {
            isStopped = false;
            workName = config.BackupName;
            copyModels.Add(this);
            TotalBytes = 0;
            int oldProgress = 0;
            int nbfiles = 0;
            var localWorker = sender as BackgroundWorker;
            localWorker.ReportProgress(0, string.Format($"{config.BackupName}"));
            // Effectuer la sauvegarde si possible

            if (!_filtersModel.CheckBusinessApp())
            {
                LangHelper langHelper = new LangHelper();
                copyModels.Remove(this);

                throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
            }

            string sourceFile = config.SourceDirectory;
            string targetFile = config.TargetDirectory;
            var logJsonModel = new LogJsonModel();
            var statelog = new Statelog();
            bool state = false;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (sourceFile != null)
            {
                var dir = new DirectoryInfo(sourceFile);
                //Passe dans la boucle si il s'agit d'un simple fichier
                if (!dir.Exists)
                {


                    var fileinfo = new FileInfo(sourceFile);
                    long fileSize = fileinfo.Length;
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
                                state = true;
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
                                    statelog.SaveLog(fileSize, byteTimeElapsed, GetDirectorySize(dir), nbfiles, EncryptionFile(sourceFile, targetFile), config, state);
                                }
                            }

                        }
                    }

                    EncryptionRecursiveFile(null, sourceFile, targetFile);
                    watch.Stop();
                    double timeElapsed = watch.Elapsed.TotalSeconds;

                    logJsonModel.SaveLog(fileSize, timeElapsed, EncryptionFile(sourceFile, targetFile), config);
                    localWorker.ReportProgress(100, string.Format($"{config.BackupName}"));


                }
                //Sinon passe dans la boucle pour copier les dossiers
                else
                {


                    DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFile);
                    long totalSize = GetDirectorySize(sourceFolderInfo);

                    if (isStopped)
                    {
                        copyModels.Remove(this);
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }
                    CopyDirectory(sourceFolderInfo, targetFile, totalSize);

                    if(isStopped)
                    {
                        copyModels.Remove(this);
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }

                    EncryptionRecursiveFile(sourceFolderInfo, sourceFile, targetFile);
                    if (isStopped)
                    {
                        copyModels.Remove(this);
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }
                    watch.Stop();
                    double timeElapsed = watch.Elapsed.TotalSeconds;
                    logJsonModel.SaveLog(totalSize, timeElapsed, EncryptionFile(sourceFile, targetFile), config);
                    statelog.SaveLog(totalSize, timeElapsed, GetDirectorySize(dir), nbfiles, EncryptionFile(sourceFile, targetFile), config, state);
                    localWorker.ReportProgress(100, string.Format($"{config.BackupName}"));




                }

                copyModels.Remove(this);
            }

            void EncryptionRecursiveFile(DirectoryInfo? sourceFolderInfo, string sourceFile, string targetFile)
            {
                if (sourceFolderInfo != null)
                {
                    if (isStopped)
                    {
                        return;
                    }
                    foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
                    {
                        if (isStopped)
                        {
                            return;
                        }
                        var source = fileInfo.FullName;
                        var target = Path.Combine(targetFile, fileInfo.Name);
                        EncryptionFile(source, target);
                    }

                    foreach (DirectoryInfo subDir in sourceFolderInfo.GetDirectories())
                    {
                        if (isStopped)
                        {
                            return;
                        }
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
                if (!_filtersModel.CheckBusinessApp())
                {
                    LangHelper langHelper = new LangHelper();
                    throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                }

                // Création du dossier cible s'il n'existe pas
                Directory.CreateDirectory(targetFolder);
                var test = sourceDirectoryInfo.GetFiles();
                List<FileInfo> filesPrioritized = _filtersModel.CheckSizeAndPriority(sourceDirectoryInfo.FullName);

                // Boucle de copie des fichiers
                foreach (var fileInfo in filesPrioritized)
                {
                    if (isPaused)
                    {
                        while (isPaused)
                        {
                            Thread.Sleep(100);
                        }
                    }
                     if (isStopped)
                    {
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }
                    nbfiles++;

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
                    if (isPaused)
                    {
                        while (isPaused)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    if (isStopped)
                    {
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }

                }
                foreach (DirectoryInfo subDir in sourceDirectoryInfo.GetDirectories())
                {
                    if (isPaused)
                    {
                        while (isPaused)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    if (isStopped)
                    {
                        return;
                    }
                    if (!_filtersModel.CheckBusinessApp())
                    {
                        LangHelper langHelper = new LangHelper();
                        copyModels.Remove(this);

                        throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
                    }
                    string newDestinationDir = Path.Combine(targetFolder, subDir.Name);
                    CopyDirectory(subDir, newDestinationDir, totalSize);
                   
                }
                if (isPaused)
                {
                    while (isPaused)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (isStopped)
                {
                    return;
                }
                if (!_filtersModel.CheckBusinessApp())
                {
                    LangHelper langHelper = new LangHelper();
                    copyModels.Remove(this);

                    throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
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
            if (_filtersModel.CheckBusinessApp())
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

                    if (isPaused)
                    {
                        while (isPaused)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    else if (isStopped)
                    {
                        copyModels.Remove(this);
                        return;
                    }

                }
            }
            else
            {
                LangHelper langHelper = new LangHelper();
                throw new Exception($"{langHelper._rm.GetString("ErrorApp")}");
            }
        }

        //Méthode qui gère le chiffrement de fichier un à un
        public TimeSpan EncryptionFile(string source, string destination)
        {
            FileEncryptModel fileEncryptModel = new FileEncryptModel();
            DateTime dateTimeStart = DateTime.Now;


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
            DateTime dateTimeEND = DateTime.Now;

            return dateTimeEND.Subtract(dateTimeStart); ;
        }

        public async Task PauseCurrentThread()
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                isPaused = true;
            }
        }

        public async Task StopCurrentThread()
        {

            isStopped = true;

        }
    }
}



