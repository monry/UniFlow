using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Empty", (int) ConnectorType.Receiver)]
    public class Empty : ReceiverBase
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            // Do nothing.
        }
    }
}