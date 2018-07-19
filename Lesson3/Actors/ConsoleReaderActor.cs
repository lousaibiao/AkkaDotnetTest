using Akka.Actor;
using System;

namespace Lesson3.Actors
{
    public class ConsoleReaderActor:UntypedActor
    {
        public const string Start = "start";
        public const string Exit = "exit";
        private readonly IActorRef validator;

        public ConsoleReaderActor(IActorRef validator)
        {
            this.validator = validator;
        }
        protected override void OnReceive(object message)
        {
            if (message.Equals(Start))
            {
                DoPrintInstructions();
            }
            GetAndValidateInput();
        }
        
        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        private void GetAndValidateInput()
        {
            var msg = Console.ReadLine();
            if (msg == Exit)
            {
                Context.System.Terminate();
            }
            validator.Tell(msg);
        }
    }
}