using UniFlow.Connector.ValueProvider;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/TimelineAssetListSelector", (int) ConnectorType.TimelineAssetListSelector)]
    public class TimelineAssetListSelector : ListSelectorBase<TimelineAsset>
    {
    }
}
