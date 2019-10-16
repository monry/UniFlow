using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/AnimationTrack", (int) ConnectorType.ValueInjectorTimelineAnimationTrack)]
    public class AnimationTrackInjector : TimelineInjectorBase<AnimationPlayableAsset>,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private string trackName = default;
        [SerializeField] private string clipName = default;
        [SerializeField] private AnimationClip animationClip = default;

        [ValueReceiver] public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public override PlayableDirector PlayableDirector
        {
            get => playableDirector != default ? playableDirector : playableDirector = this.GetOrAddComponent<PlayableDirector>();
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

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private PlayableDirectorCollector playableDirectorCollector = default;
        [SerializeField] private StringCollector trackNameCollector = default;
        [SerializeField] private StringCollector clipNameCollector = default;
        [SerializeField] private AnimationClipCollector animationClipCollector = default;

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private PlayableDirectorCollector PlayableDirectorCollector => playableDirectorCollector;
        private StringCollector TrackNameCollector => trackNameCollector;
        private StringCollector ClipNameCollector => clipNameCollector;
        private AnimationClipCollector AnimationClipCollector => animationClipCollector;

        protected override void Inject(AnimationPlayableAsset playableAsset)
        {
            playableAsset.clip = AnimationClip;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<GameObject>(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                new CollectableMessageAnnotation<string>(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                new CollectableMessageAnnotation<PlayableDirector>(PlayableDirectorCollector, x => PlayableDirector = x, nameof(PlayableDirector)),
                new CollectableMessageAnnotation<string>(TrackNameCollector, x => TrackName = x, nameof(TrackName)),
                new CollectableMessageAnnotation<string>(ClipNameCollector, x => ClipName = x, nameof(ClipName)),
                new CollectableMessageAnnotation<AnimationClip>(AnimationClipCollector, x => AnimationClip = x, nameof(AnimationClip)),
            };
    }
}
