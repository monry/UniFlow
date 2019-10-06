using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Object", (int) ConnectorType.ValueProviderObject)]
    public class ObjectProvider : ValueProviderBase<Object, PublishObjectEvent>
    {
        protected override Object Provide()
        {
            return Value;
        }
    }
}
