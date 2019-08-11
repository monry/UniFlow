using System.Text;
using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Receiver/Log", 20001)]
    public class Log : ReceiverBase
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>EventCount</b>: {eventMessages.Count}");
            sb.AppendLine("<b>EventMessages</b>:");
            var index = 0;
            foreach (var eventMessage in eventMessages)
            {
                sb.AppendLine($"  {index}");
                sb.AppendLine($"    <b>Type</b>:\n      {eventMessage.ConnectorType}");
                sb.AppendLine($"    <b>Sender</b>:\n      {eventMessage.Sender.ToString().Replace("\n", "\n      ")}");
                sb.AppendLine($"    <b>Data</b>:\n      {eventMessage.Data.ToString().Replace("\n", "\n      ")}");
                index++;
            }
            Debug.Log(sb.ToString());
        }
    }
}