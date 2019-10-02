using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Object", (int) ConnectorType.ValueProviderObject)]
    public class ObjectProvider : ProviderBase<Object, PublishObjectEvent>
    {
        [SerializeField] private Object value = default;
        private Object Value => value;

        protected override Object Provide()
        {
            return Value;
        }
    }
}
