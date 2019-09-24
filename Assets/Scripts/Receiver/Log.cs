using System;
using System.Reflection;
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
            foreach (var message in messages)
            {
                sb.AppendLine($"  {index}");
                sb.AppendLine($"    <b>Type</b>:\n      {message.ConnectorType}");
                sb.AppendLine($"    <b>Message Type</b>:\n      {message.GetType()}");
                if (message.GetType().BaseType is Type baseType && baseType.IsGenericType && baseType.GenericTypeArguments.Length == 2)
                {
                    var data = message.GetType().GetProperty("Data", BindingFlags.Instance | BindingFlags.Public)?.GetValue(message);
                    sb.AppendLine($"    <b>Data</b>:\n      <b>Type</b>:\n        {data?.GetType()}\n      <b>Value</b>:\n        {data?.ToString().Replace("\n", "\n        ")}");
                }
                index++;
            }
            Debug.Log(sb.ToString());
        }
    }
}