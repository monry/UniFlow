using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/GameObjectProvider", (int) ConnectorType.ValueProviderGameObject)]
    public class GameObjectProvider : ProviderBase<GameObject>
    {
    }
}
