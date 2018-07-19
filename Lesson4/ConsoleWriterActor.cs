using System;
using Akka.Actor;

namespace Lesson4
{
    public class ConsoleWriterActor:UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is Messages.InputError ieMsg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ieMsg.Reason);
            }
            else if (message is Messages.InputSuccess isMsg)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(isMsg.Reason);
            }
            else
            {
                Console.WriteLine(message);
            }
            
            Console.ResetColor();
        }
    }
}