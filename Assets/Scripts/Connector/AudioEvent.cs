using System;
using EventConnector.Message;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEvent : EventPublisher
    {
        [SerializeField] private AudioEventType audioEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<AudioSource>()")]
        private AudioSource audioSource = default;

        private AudioEventType AudioEventType => audioEventType;
        private AudioSource AudioSource => audioSource ? audioSource : audioSource = GetComponent<AudioSource>();

        private IReadOnlyReactiveProperty<Pair<float>> TimePair { get; set; }

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnAudioEventAsObservable()
                .Select(x => EventMessage.Create(EventType.AudioEvent, AudioSource, AudioEventData.Create(x)));

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

        private AudioEventType DetectAudioEventType(bool isPlaying, AudioClip audioClip)
        {
            if (audioClip == default)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (isPlaying && Mathf.Approximately(0.0f, TimePair.Value.Previous) && TimePair.Value.Current > 0.0f)
            {
                return AudioEventType.Play;
            }

            if (!isPlaying && (Mathf.Approximately(audioClip.length, TimePair.Value.Current) || Mathf.Approximately(0.0f, TimePair.Value.Current) && TimePair.Value.Previous > 0.0f))
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