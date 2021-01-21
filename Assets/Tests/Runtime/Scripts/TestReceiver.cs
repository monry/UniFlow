using System.Collections.Generic;
using UniRx;

namespace UniFlow.Tests.Runtime
{
    public class TestReceiver : ReceiverBase
    {
        public IList<IConnector> SentConnectors { get; } = new List<IConnector>();
        public int ReceiveCount { get; private set; }

        private void Awake()
        {
            Logger.OnMessageAsObservable().Where(x => x != default).Subscribe(SentConnectors.Add);
        }

        public void Reset()
        {
            SentConnectors.Clear();
        }

        public override void OnReceive()
        {
            ReceiveCount++;
        }
    }
}
