using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/AudioEvent", (int) ConnectorType.AudioEvent)]
    public class AudioEvent : ConnectorBase
    {
        [SerializeField] private AudioEventType audioEventType = (AudioEventType) (-1);
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by AudioSource.clip")]
        private AudioClip audioClip = default;
        [SerializeField] private AudioSource audioSource = default;

        [UsedImplicitly] public AudioEventType AudioEventType
        {
            get => audioEventType;
            set => audioEventType = value;
        }
        [UsedImplicitly] public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }
        [UsedImplicitly] public AudioSource AudioSource
        {
            get =>
                audioSource != default
                    ? audioSource
                    : audioSource =
                        GetComponent<AudioSource>() != default
                            ? GetComponent<AudioSource>()
                            : gameObject.AddComponent<AudioSource>();
            set => audioSource = value;
        }

        private IReadOnlyReactiveProperty<Pair<float>> TimePair { get; set; }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnAudioEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.AudioEvent, AudioSource, AudioEventData.Create(x)));
        }

        private void Awake()
        {
            if (AudioClip != default)
            {
                AudioSource.clip = AudioClip;
            }
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
                        .Select(x => DetectAudioEventType(x, AudioSource.clip))
                        .Where(x => AudioEventType == x);
                case AudioEventType.Loop:
                    return TimePair
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