using System.Collections.Generic;
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

        public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        public string TransformPath
        {
            get => transformPath;
            private set => transformPath = value;
        }
        protected override PlayableDirector PlayableDirector
        {
            get => playableDirector != default ? playableDirector : playableDirector = this.GetOrAddComponent<PlayableDirector>();
            set => playableDirector = value;
        }
        protected override string TrackName
        {
            get => trackName;
            set => trackName = value;
        }
        protected override string ClipName
        {
            get => clipName;
            set => clipName = value;
        }
        private AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private PlayableDirectorCollector playableDirectorCollector = new PlayableDirectorCollector();
        [SerializeField] private StringCollector trackNameCollector = new StringCollector();
        [SerializeField] private StringCollector clipNameCollector = new StringCollector();
        [SerializeField] private AnimationClipCollector animationClipCollector = new AnimationClipCollector();

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
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(PlayableDirectorCollector, x => PlayableDirector = x, nameof(PlayableDirector)),
                CollectableMessageAnnotationFactory.Create(TrackNameCollector, x => TrackName = x, nameof(TrackName)),
                CollectableMessageAnnotationFactory.Create(ClipNameCollector, x => ClipName = x, nameof(ClipName)),
                CollectableMessageAnnotationFactory.Create(AnimationClipCollector, x => AnimationClip = x, nameof(AnimationClip)),
            };
    }
}
