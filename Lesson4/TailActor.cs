using Akka.Actor;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Lesson4
{
    public class TailActor : UntypedActor
    {
        #region Message types

        public class FileWrite
        {
            public FileWrite(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }

        public class FileError
        {
            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }

            public string FileName { get; }
            public string Reason { get; }
        }

        public class InitialRead
        {
            public InitialRead(string fileName, string text)
            {
                FileName = fileName;
                Text = text;
            }

            public string FileName { get; }
            public string Text { get; }
        }

        #endregion

        private readonly string filePath;
        private readonly IActorRef reporterActor;
        private readonly FileObserver observer;
        private readonly Stream fileStream;
        private readonly StreamReader fileStreamReader;

        public TailActor(IActorRef reporterActor, string filePath)
        {
            this.reporterActor = reporterActor;
            this.filePath = filePath;
            this.observer = new FileObserver(Self, Path.GetFullPath(filePath));
            this.observer.Start();

            this.fileStream =
                new FileStream(Path.Combine(filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);

            var text = fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(filePath, text));
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                var text = fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    reporterActor.Tell(text);
                }
                
            }else if (message is FileError fe)
            {
                reporterActor.Tell($"Tail error :{fe.Reason}");
            }else if (message is InitialRead ir)
            {
                reporterActor.Tell(ir.Text);
            }
        }
    }
}