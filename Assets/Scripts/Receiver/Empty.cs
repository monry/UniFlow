using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Empty", 20000)]
    public class Empty : ReceiverBase
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            // Do nothing.
        }
    }
}