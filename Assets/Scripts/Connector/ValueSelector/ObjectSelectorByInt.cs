using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/ObjectSelectorByInt", (int) ConnectorType.ObjectSelectorByInt)]
    public class ObjectSelectorByInt : SelectorBase<int, Object, IntCollector>
    {
    }
}
