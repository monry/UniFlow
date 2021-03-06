using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/AudioEvent", (int) ConnectorType.AudioEvent)]
    public class AudioEvent : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private AudioEventType audioEventType = AudioEventType.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by AudioSource.clip")]
        private AudioClip audioClip = default;

        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            private set => baseGameObject = value;
        }
        public string TransformPath
        {
            get => transformPath;
            private set => transformPath = value;
        }
        public AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = this.GetOrAddComponent<AudioSource>();
            private set => audioSource = value;
        }
        public AudioEventType AudioEventType => audioEventType;
        private AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private AudioSourceCollector audioSourceCollector = new AudioSourceCollector();
        [SerializeField] private AudioClipCollector audioClipCollector = new AudioClipCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private AudioClipCollector AudioClipCollector => audioClipCollector;

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(AudioSourceCollector, x => AudioSource = x),
                CollectableMessageAnnotationFactory.Create(AudioClipCollector, x => AudioClip = x),
            };
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
