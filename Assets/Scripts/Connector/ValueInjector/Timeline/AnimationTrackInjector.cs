using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/AnimationTrack", (int) ConnectorType.ValueInjectorTimelineAnimationTrack)]
    public class AnimationTrackInjector : TimelineInjectorBase<AnimationPlayableAsset>
    {
        [SerializeField] private AnimationClip animationClip = default;

        [ValueReceiver] public AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }

        protected override void Inject(AnimationPlayableAsset playableAsset)
        {
            playableAsset.clip = AnimationClip;
        }
    }
}
