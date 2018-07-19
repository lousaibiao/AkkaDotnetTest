using System;
using Akka.Actor;

namespace Lesson2
{
    class Program
    {

        public static ActorSystem myActorSystem;
        static void Main(string[] args)
        {
            myActorSystem = ActorSystem.Create("MyActorSystem");
//            Props.Create<Person>();
            var consoleWriterActor = myActorSystem.ActorOf(Props.Create(()=>new ConsoleWriterActor()),"consoleWriteActor");
            var consoleReaderActor= myActorSystem.ActorOf(Props.Create(()=>new ConsoleReaderActor(consoleWriterActor)),"consoleReaderActor");

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            myActorSystem.WhenTerminated.Wait();
        }
    }

    public class Person
    {
        
    }
}
