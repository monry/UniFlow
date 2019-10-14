using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/AudioEvent", (int) ConnectorType.AudioEvent)]
    public class AudioEvent : ConnectorBase, IBaseGameObjectSpecifyable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private AudioEventType audioEventType = AudioEventType.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by AudioSource.clip")]
        private AudioClip audioClip = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = this.GetOrAddComponent<AudioSource>();
            set => audioSource = value;
        }
        [UsedImplicitly] public AudioEventType AudioEventType
        {
            get => audioEventType;
            set => audioEventType = value;
        }
        [ValueReceiver] public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }

        private IReadOnlyReactiveProperty<Pair<float>> TimePair { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnAudioEventAsObservable()
                .Select(this.CreateMessage);
        }

        private IObservable<AudioEventType> OnAudioEventAsObservable()
        {
            TimePair = AudioSource
                .ObserveEveryValueChanged(x => x.time)
                .Pairwise()
                .ToReactiveProperty();
            switch (AudioEventType)
            {
                case AudioEventType.Play:
                case AudioEventType.Stop:
                case AudioEventType.Pause:
                case AudioEventType.UnPause:
                    return AudioSource
                        .ObserveEveryValueChanged(x => x.isPlaying)
                        .Where(_ => AudioClip == default || AudioClip == AudioSource.clip)
                        .Select(x => DetectAudioEventType(x, AudioSource.clip))
                        .Where(x => AudioEventType == x);
                case AudioEventType.Loop:
                    return TimePair
                        .Where(_ => AudioClip == default || AudioClip == AudioSource.clip)
                        .Where(xs => AudioSource.isPlaying && xs.Current < xs.Previous && xs.Previous > 0.0f)
                        .Select(_ => AudioEventType.Loop);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AudioEventType DetectAudioEventType(bool isPlaying, AudioClip clip)
        {
            if (clip == default)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (isPlaying && Mathf.Approximately(0.0f, TimePair.Value.Previous) && TimePair.Value.Current > 0.0f)
            {
                return AudioEventType.Play;
            }

            if (!isPlaying && (Mathf.Approximately(clip.length, TimePair.Value.Current) || Mathf.Approximately(0.0f, TimePair.Value.Current) && TimePair.Value.Previous > 0.0f))
            {
                return AudioEventType.Stop;
            }

            if (!isPlaying && TimePair.Value.Current > 0.0f && TimePair.Value.Current > TimePair.Value.Previous)
            {
                return AudioEventType.Pause;
            }

            if (isPlaying && TimePair.Value.Current > 0.0f && TimePair.Value.Current > TimePair.Value.Previous)
            {
                return AudioEventType.UnPause;
            }

            return (AudioEventType) -1;
        }
    }

    public enum AudioEventType
    {
        Play,
        Stop,
        Pause,
        UnPause, // Follow method name in AudioClip
        Loop,
    }
}
