using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/GameObject", (int) ConnectorType.ValueProviderGameObject)]
    public class GameObjectProvider : ProviderBase<GameObject>
    {
        [SerializeField] private GameObject value = default;
        private GameObject Value => value;

        [SerializeField] private PublishGameObjectEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<GameObject> Publisher => publisher ?? (publisher = new PublishGameObjectEvent());

        protected override GameObject Provide()
        {
            return Value;
        }
    }
}
