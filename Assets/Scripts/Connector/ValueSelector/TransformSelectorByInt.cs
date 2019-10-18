using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/TransformSelectorByInt", (int) ConnectorType.TransformSelectorByInt)]
    public class TransformSelectorByInt : SelectorBase<int, Transform, IntCollector>
    {
    }
}
