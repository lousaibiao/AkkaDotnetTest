using System;
using Akka.Actor;
using Lesson3.Actors;

namespace Lesson3
{
    class Program
    {
        public static ActorSystem actorSystem;
        static void Main(string[] args)
        {
            actorSystem = ActorSystem.Create("myActorSystem");
            var writeProps = Props.Create<ConsoleWriterActor>();
            var writeActor = actorSystem.ActorOf(writeProps, "writeActor");

            var validateProps = Props.Create<ConsoleValidatorActor>(writeActor);
            var validateActor = actorSystem.ActorOf(validateProps, "validateActor");

            var readerProps = Props.Create<ConsoleReaderActor>(validateActor);
            var readerActor = actorSystem.ActorOf(readerProps, "readerActor");
            
            readerActor.Tell(ConsoleReaderActor.Start);

            actorSystem.WhenTerminated.Wait();
        }
    }
}