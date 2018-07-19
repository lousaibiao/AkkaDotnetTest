using Akka.Actor;

namespace Lesson3.Actors
{
    public class ConsoleValidatorActor : UntypedActor
    {
        private readonly IActorRef writer;

        public ConsoleValidatorActor(IActorRef writer)
        {
            this.writer = writer;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                writer.Tell("请输入内容");
            }
            else
            {
                writer.Tell(msg);
            }
            Sender.Tell("继续");
        }
    }
}