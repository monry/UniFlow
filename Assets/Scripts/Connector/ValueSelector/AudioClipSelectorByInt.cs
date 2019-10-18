using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/AudioClipSelectorByInt", (int) ConnectorType.AudioClipSelectorByInt)]
    public class AudioClipSelectorByInt : SelectorBase<int, AudioClip, IntCollector>
    {
    }
}
