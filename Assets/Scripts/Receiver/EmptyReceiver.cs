using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/EmptyReceiver", (int) ConnectorType.Receiver)]
    public class EmptyReceiver : ReceiverBase
    {
        public override void OnReceive()
        {
            // Do nothing.
        }
    }
}
