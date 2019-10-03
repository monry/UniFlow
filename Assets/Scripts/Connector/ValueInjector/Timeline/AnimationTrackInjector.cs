using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/AnimationTrack", (int) ConnectorType.ValueInjectorTimelineAnimationTrack)]
    public class AnimationTrackInjector : TimelineInjectorBase<AnimationPlayableAsset>
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private string trackName = default;
        [SerializeField] private string clipName = default;
        [SerializeField] private AnimationClip animationClip = default;

        [ValueReceiver] public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public override PlayableDirector PlayableDirector
        {
            get =>
                playableDirector != default
                    ? playableDirector
                    : playableDirector =
                        BaseGameObject.GetComponent<PlayableDirector>() != default
                            ? BaseGameObject.GetComponent<PlayableDirector>()
                            : BaseGameObject.AddComponent<PlayableDirector>();
            set => playableDirector = value;
        }
        [ValueReceiver] public override string TrackName
        {
            get => trackName;
            set => trackName = value;
        }
        [ValueReceiver] public override string ClipName
        {
            get => clipName;
            set => clipName = value;
        }
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
