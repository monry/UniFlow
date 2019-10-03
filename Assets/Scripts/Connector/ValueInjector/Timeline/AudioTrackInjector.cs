using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector.Timeline
{
    [AddComponentMenu("UniFlow/ValueInjector/Timeline/AudioTrack", (int) ConnectorType.ValueInjectorTimelineAudioTrack)]
    public class AudioTrackInjector : TimelineInjectorBase<AudioPlayableAsset>
    {
        [SerializeField] private AudioClip audioClip = default;

        [ValueReceiver] public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }

        protected override void Inject(AudioPlayableAsset playableAsset)
        {
            playableAsset.clip = AudioClip;
        }
    }
}
