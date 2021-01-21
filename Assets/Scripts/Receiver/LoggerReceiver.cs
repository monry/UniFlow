using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/LoggerReceiver", (int) ConnectorType.Receiver)]
    public class LoggerReceiver : ReceiverBase
    {
        private IList<IConnector> Connectors { get; } = new List<IConnector>();

        private void Awake()
        {
            Logger.Activate();
            Logger.OnMessageAsObservable().Subscribe(Connectors.Add);
        }

        public override void OnReceive()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Count</b>: {Connectors.Count}");
            sb.AppendLine("<b>Connectors</b>:");
            foreach (var connector in Connectors)
            {
                sb.AppendLine($"  {connector.GetType().Name}");
            }
            Debug.Log(sb.ToString());
        }
    }
}
