using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/ControlTrack", (int) ConnectorType.ValueInjectorTimelineControlTrack)]
    public class ControlTrackInjector : TimelineInjectorBase<ControlPlayableAsset>
    {
        [SerializeField] private GameObject sourceGameObject = default;
        [SerializeField] private GameObject prefabGameObject = default;

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
                playableAsset.sourceGameObject.defaultValue = SourceGameObject;
            }
            if (PrefabGameObject != default)
            {
                playableAsset.prefabGameObject = PrefabGameObject;
            }
        }
    }
}
