using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/GameObjectSelectorByInt", (int) ConnectorType.GameObjectSelectorByInt)]
    public class GameObjectSelectorByInt : SelectorBase<int, GameObject, IntCollector>
    {
    }
}
