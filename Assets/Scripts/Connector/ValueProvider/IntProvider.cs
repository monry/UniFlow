using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Int", (int) ConnectorType.ValueProviderInt)]
    public class IntProvider : ValueProviderBase<int, PublishIntEvent>
    {
        protected override int Provide()
        {
            return Value;
        }
    }
}
