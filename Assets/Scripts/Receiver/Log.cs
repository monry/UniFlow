using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Log", (int) ConnectorType.Receiver)]
    public class Log : ReceiverBase
    {
        private IList<IConnector> Connectors { get; } = new List<IConnector>();

        protected override void Awake()
        {
            base.Awake();
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
