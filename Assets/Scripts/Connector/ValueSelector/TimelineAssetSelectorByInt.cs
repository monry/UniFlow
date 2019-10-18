using UniFlow.Connector.ValueProvider;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/TimelineAssetSelectorByInt", (int) ConnectorType.TimelineAssetSelectorByInt)]
    public class TimelineAssetSelectorByInt : SelectorBase<int, TimelineAsset, IntCollector>
    {
    }
}
