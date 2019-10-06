using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class StringProvider : ValueProviderBase<string, PublishStringEvent>
    {
        protected override string Provide()
        {
            return Value;
        }
    }
}
