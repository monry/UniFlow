using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/GameObject", (int) ConnectorType.ValueProviderGameObject)]
    public class GameObjectProvider : ProviderBase<GameObject, PublishGameObjectEvent>
    {
        [SerializeField] private GameObject value = default;
        private GameObject Value => value;

        protected override GameObject Provide()
        {
            return Value;
        }
    }
}
