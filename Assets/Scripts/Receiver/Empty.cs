using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Empty", (int) ConnectorType.Receiver)]
    public class Empty : ReceiverBase
    {
        public override void OnReceive(Messages messages)
        {
            // Do nothing.
        }
    }
}