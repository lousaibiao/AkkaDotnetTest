using System;
using Akka.Actor;

namespace WinTail
{
    class Program
    {
        public static ActorSystem myActorSystem;
        static void Main(string[] args)
        {

            myActorSystem = ActorSystem.Create("MyActorSystem");

            PrintInstructions();

            var consoleWriteActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "consoleWriterActor");
            var consoleReadActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriteActor)), "consoleReadActor");

            //consoleReadActor.Tell("start");

            myActorSystem.WhenTerminated.Wait();


            Console.WriteLine("Hello World!");
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
}