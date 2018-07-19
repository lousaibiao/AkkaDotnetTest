using Akka.Actor;

using System;
namespace Lesson2
{
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is Messages.InputError msg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.Reason);
            }
            else if (message is Messages.InputSuccess sucMsg)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(sucMsg.Reason);
            }
            else
            {
                System.Console.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}