using System.Text;
using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Log", (int) ConnectorType.Receiver)]
    public class Log : ReceiverBase
    {
        public override void OnReceive(Messages messages)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>EventCount</b>: {messages.Count}");
            sb.AppendLine("<b>Messages</b>:");
            var index = 0;
            foreach (var eventMessage in messages)
            {
                sb.AppendLine($"  {index}");
                sb.AppendLine($"    <b>Type</b>:\n      {eventMessage.ConnectorType}");
                // TODO: データの送り方を考える
//                sb.AppendLine($"    <b>Data</b>:\n      {eventMessage.Data?.ToString().Replace("\n", "\n      ")}");
                index++;
            }
            Debug.Log(sb.ToString());
        }
    }
}