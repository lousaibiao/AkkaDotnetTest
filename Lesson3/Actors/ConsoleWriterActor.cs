using Akka.Actor;
using System;

namespace Lesson3.Actors
{
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var msg = message as string;
            Console.WriteLine(msg);
        }
    }
}