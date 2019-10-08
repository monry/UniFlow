using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ValueProviderBase<Color32, PublishColor32Event>
    {
        protected override Color32 Provide()
        {
            return Value;
        }
    }
}
