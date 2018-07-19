using System;
using Akka.Actor;

namespace Lesson4
{
    public class ConsoleReaderActor:UntypedActor
    {
        private readonly IActorRef validatorActor;
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";

        public ConsoleReaderActor(IActorRef validatorActor)
        {
            this.validatorActor = validatorActor;
        }
        protected override void OnReceive(object message)
        {
            if (message == StartCommand)
            {
                DoPrintInstructions();
            }
            GetAndValidateInput();
        }

        #region internal methods

        private void DoPrintInstructions()
        {
            Console.WriteLine("Pls provide the uri of a log file on disk");
        }

        private void GetAndValidateInput()
        {
            var msg = Console.ReadLine();
            if (!String.IsNullOrEmpty(msg) && msg == ExitCommand)
            {
                Context.System.Terminate();
                return;
            }
            validatorActor.Tell(msg);
        }

        #endregion
    }
}