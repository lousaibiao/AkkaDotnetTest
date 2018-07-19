using Akka.Actor;

namespace Lesson4
{
    /// <summary>
    /// parent actor
    /// </summary>
    public class TailCoordinatorActor : UntypedActor
    {
        #region message types
        public class StartTail
        {

            public StartTail(string filePath, IActorRef reportActor)
            {
                FilePath = filePath;
                ReportActor = reportActor;
            }

            public string FilePath { get; }
            public IActorRef ReportActor { get; }
        }

        public class StopTail
        {
            public StopTail(string filePath)
            {
                FilePath = filePath;
            }

            public string FilePath { get; }
        }

        #endregion
        protected override void OnReceive(object message)
        {
            if (message is StartTail msg)
            {
                Context.ActorOf(Props.Create(() => new TailActor(msg.ReportActor, msg.FilePath)));
            }
            // throw new System.NotImplementedException();
        }
    }
}