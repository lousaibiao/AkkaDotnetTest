using System;
using System.IO;
using Akka.Actor;
using Akka.IO;

namespace Lesson4
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef consoleWriterActor;
        private readonly IActorRef tailCooridatorActor;

        public FileValidatorActor(IActorRef consoleWriterActor, IActorRef tailCooridatorActor)
        {
            this.consoleWriterActor = consoleWriterActor;
            this.tailCooridatorActor = tailCooridatorActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (String.IsNullOrEmpty(msg))
            {
                consoleWriterActor.Tell(new Messages.NullInputError($"Input is blank, pls try again!"));
                Sender.Tell(new Messages.ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(msg);
                if (valid)
                {
                    consoleWriterActor.Tell(new Messages.InputSuccess($"Start processing for {msg}"));
                    tailCooridatorActor.Tell(new TailCoordinatorActor.StartTail(msg, consoleWriterActor));
                }
                else
                {
                    consoleWriterActor.Tell(new Messages.ValidationError($"{msg} is not an exist URI on disk"));
                    Sender.Tell(new Messages.ContinueProcessing());
                }

            }
        }

        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}