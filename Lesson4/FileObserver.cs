using System;
using System.IO;
using Akka.Actor;

namespace Lesson4
{
    public class FileObserver : IDisposable
    {
        private readonly IActorRef tailActor;
        private readonly string absoluteFilePath;

        private FileSystemWatcher watcher;

        private readonly string fileDir;
        private readonly string fileNameOnly;
        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            this.tailActor = tailActor;
            this.absoluteFilePath = absoluteFilePath;
            this.fileDir = Path.GetDirectoryName(absoluteFilePath);
            this.fileNameOnly = Path.GetFileName(absoluteFilePath);
        }
        public void Start()
        {
            watcher = new FileSystemWatcher(fileDir, fileNameOnly);
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            watcher.Changed += OnFileChanged;
            watcher.Error += OnFileError;

            watcher.EnableRaisingEvents = true;
        }
        public void Dispose()
        {
            watcher.Dispose();
        }

        void OnFileError(object sender, ErrorEventArgs e)
        {
            tailActor.Tell(new TailActor.FileError(fileNameOnly, e.GetException().Message), ActorRefs.NoSender);
        }
        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                tailActor.Tell(new TailActor.FileWrite(e.Name), ActorRefs.NoSender);
            }
        }
    }
}