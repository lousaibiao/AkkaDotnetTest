using System;
using Akka.Actor;

namespace Lesson4
{
    class Program
    {
        private static readonly ActorSystem myActorSystem = ActorSystem.Create("myActorSystem");

        static void Main(string[] args)
        {
            Props consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
            var consoleWriterActor = myActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props tailCooridatorProps = Props.Create(() => new TailCoordinatorActor());
            var tailCooridatorActor = myActorSystem.ActorOf(tailCooridatorProps, "tailCoordinatorActor");

            Props fileValidatorActorProps =
                Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCooridatorActor));
            var fileValidatorActor = myActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            Props consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidatorActor);
            var consoleReaderActor = myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");
            
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);
            myActorSystem.WhenTerminated.Wait();
            
        }
    }
}