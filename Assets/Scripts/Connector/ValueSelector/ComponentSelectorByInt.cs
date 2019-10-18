using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/ComponentSelectorByInt", (int) ConnectorType.ComponentSelectorByInt)]
    public class ComponentSelectorByInt : SelectorBase<int, Component, IntCollector>
    {
    }
}
