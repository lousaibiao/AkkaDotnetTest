using System;
using Akka.Actor;

namespace WinTail
{
    public class ConsoleReaderActor : UntypedActor
    {
        private readonly IActorRef consoleWriterActor;
        public const string ExitCommand = "exit";

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var read = Console.ReadLine();
            if (!String.IsNullOrEmpty(read) && String.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }

            consoleWriterActor.Tell(read);

            Self.Tell("continue");
            //Unhandled((""));
        }
    }
}