using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/ControlTrack", (int) ConnectorType.ValueInjectorTimelineControlTrack)]
    public class ControlTrackInjector : TimelineInjectorBase<ControlPlayableAsset>,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private string trackName = default;
        [SerializeField] private string clipName = default;
        [SerializeField] private GameObject sourceGameObject = default;
        [SerializeField] private GameObject prefabGameObject = default;

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
        public GameObject SourceGameObject
        {
            get => sourceGameObject;
            set => sourceGameObject = value;
        }
        private GameObject PrefabGameObject
        {
            get => prefabGameObject;
            set => prefabGameObject = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private PlayableDirectorCollector playableDirectorCollector = default;
        [SerializeField] private StringCollector trackNameCollector = default;
        [SerializeField] private StringCollector clipNameCollector = default;
        [SerializeField] private GameObjectCollector sourceGameObjectCollector = default;
        [SerializeField] private GameObjectCollector prefabGameObjectCollector = default;

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private PlayableDirectorCollector PlayableDirectorCollector => playableDirectorCollector;
        private StringCollector TrackNameCollector => trackNameCollector;
        private StringCollector ClipNameCollector => clipNameCollector;
        private GameObjectCollector SourceGameObjectCollector => sourceGameObjectCollector;
        private GameObjectCollector PrefabGameObjectCollector => prefabGameObjectCollector;

        protected override void Inject(ControlPlayableAsset playableAsset)
        {
            if (SourceGameObject != default)
            {
                PlayableDirector.SetReferenceValue(playableAsset.sourceGameObject.exposedName, SourceGameObject);
            }
            if (PrefabGameObject != default)
            {
                playableAsset.prefabGameObject = PrefabGameObject;
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<GameObject>.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotation<string>.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotation<PlayableDirector>.Create(PlayableDirectorCollector, x => PlayableDirector = x, nameof(PlayableDirector)),
                CollectableMessageAnnotation<string>.Create(TrackNameCollector, x => TrackName = x, nameof(TrackName)),
                CollectableMessageAnnotation<string>.Create(ClipNameCollector, x => ClipName = x, nameof(ClipName)),
                CollectableMessageAnnotation<GameObject>.Create(SourceGameObjectCollector, x => SourceGameObject = x, nameof(SourceGameObject)),
                CollectableMessageAnnotation<GameObject>.Create(PrefabGameObjectCollector, x => PrefabGameObject = x, nameof(PrefabGameObject)),
            };
    }
}
