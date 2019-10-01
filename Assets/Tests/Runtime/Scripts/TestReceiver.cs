using System.Collections.Generic;
using UniRx;

namespace UniFlow.Tests.Runtime
{
    public class TestReceiver : ReceiverBase
    {
        public IList<IConnector> SentConnectors { get; private set; } = new List<IConnector>();
        public int ReceiveCount { get; private set; }
        
        protected override void Awake()
        {
            Logger.Activate();
            Logger.OnMessageAsObservable().Subscribe(SentConnectors.Add);
        }

        public override void OnReceive()
        {
            ReceiveCount++;
        }
    }
}
