using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/ControlTrack", (int) ConnectorType.ValueInjectorTimelineControlTrack)]
    public class ControlTrackInjector : TimelineInjectorBase<ControlPlayableAsset>, IBaseGameObjectSpecifyable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private string trackName = default;
        [SerializeField] private string clipName = default;
        [SerializeField] private GameObject sourceGameObject = default;
        [SerializeField] private GameObject prefabGameObject = default;

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
        [ValueReceiver] public GameObject SourceGameObject
        {
            get => sourceGameObject;
            set => sourceGameObject = value;
        }
        [ValueReceiver] public GameObject PrefabGameObject
        {
            get => prefabGameObject;
            set => prefabGameObject = value;
        }

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
    }
}
