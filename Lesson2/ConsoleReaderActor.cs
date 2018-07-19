using Akka.Actor;
using System;
namespace Lesson2
{
    public class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        private readonly IActorRef consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
        }
        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError errorMsg)
            {
                consoleWriterActor.Tell(errorMsg);
            }
            GetAndValidateInput();
        }

        #region Internal methods
        private void DoPrintInstructions()
        {
            System.Console.WriteLine("Write whatever you want into the console");
            System.Console.WriteLine($"Some entries will pass validation, and some won't{Environment.NewLine}");
            System.Console.WriteLine($"Type {ExitCommand} to quit this application at any time {Environment.NewLine}");
        }

        private void GetAndValidateInput()
        {
            var msg = Console.ReadLine();
            if (string.IsNullOrEmpty(msg))
            {
                Self.Tell(new Messages.NullInputError("No input received"));
            }
            else if (string.Equals(msg, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
            }
            else
            {
                if (IsValid(msg))
                {
                    Self.Tell(new Messages.ContinueProcessing());
                    consoleWriterActor.Tell(new Messages.InputSuccess("Thank you, msg is valid"));
                }
                else
                {
                    Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
                }

            }
        }
        private bool IsValid(string msg)
        {
            return msg.Length % 2 == 0;
        }
        #endregion
    }
}