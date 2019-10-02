using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class BoolProvider : ProviderBase<bool, PublishBoolEvent>
    {
        [SerializeField] private bool value = default;
        private bool Value => value;

        protected override bool Provide()
        {
            return Value;
        }
    }
}
