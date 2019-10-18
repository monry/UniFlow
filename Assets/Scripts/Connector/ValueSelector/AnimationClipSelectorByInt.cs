using UniFlow.Connector.ValueProvider;
using UnityEngine;

namespace UniFlow.Connector.ValueSelector
{
    [AddComponentMenu("UniFlow/ValueSelector/AnimationClipSelectorByInt", (int) ConnectorType.AnimationClipSelectorByInt)]
    public class AnimationClipSelectorByInt : SelectorBase<int, AnimationClip, IntCollector>
    {
    }
}
