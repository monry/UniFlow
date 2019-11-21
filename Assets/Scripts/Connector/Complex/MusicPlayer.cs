using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniFlow;
using UniFlow.Connector.Controller;
using UniFlow.Utility;
using UnityEngine;

namespace PretendLand.Edwin.Scripts.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/MusicPlayer", (int) ConnectorType.MusicPlayer)]
    public class MusicPlayer : ConnectorBase, IMessageCollectable
    {
        internal static GameObject BaseGameObject { get; private set; }

        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private AudioControlMethod audioControlMethod = AudioControlMethod.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by AudioSource.clip")]
        private AudioClip audioClip = default;
        [SerializeField] private string key = default;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool rewindSameClip = default;

        private AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = BaseGameObject.GetOrAddComponent<AudioSource>();
            set => audioSource = value;
        }
        private AudioControlMethod AudioControlMethod
        {
            get => audioControlMethod;
            set => audioControlMethod = value;
        }
        private AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }
        [UsedImplicitly] private string Key
        {
            get => string.IsNullOrEmpty(key) ? AudioClip.name : key;
            set => key = value;
        }
        private bool Loop
        {
            get => loop;
            set => loop = value;
        }
        private bool RewindSameClip
        {
            get => rewindSameClip;
            set => rewindSameClip = value;
        }

        [SerializeField] private AudioSourceCollector audioSourceCollector = new AudioSourceCollector();
        [SerializeField] private AudioControlMethodCollector audioControlMethodCollector = new AudioControlMethodCollector();
        [SerializeField] private AudioClipCollector audioClipCollector = new AudioClipCollector();
        [SerializeField] private BoolCollector loopCollector = default;
        [SerializeField] private BoolCollector rewindSameClipCollector = new BoolCollector();

        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private AudioControlMethodCollector AudioControlMethodCollector => audioControlMethodCollector;
        private AudioClipCollector AudioClipCollector => audioClipCollector;
        private BoolCollector LoopCollector => loopCollector;
        private BoolCollector RewindSameClipCollector => rewindSameClipCollector;

        private void Awake()
        {
            BaseGameObject = gameObject;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            InvokeAudioSourceMethod();
            return ObservableFactory.ReturnMessage(this);
        }

        private void InvokeAudioSourceMethod()
        {
            switch (AudioControlMethod)
            {
                case AudioControlMethod.Play:
                    if (!AudioSource.isPlaying || AudioSource.name != Key || RewindSameClip)
                    {
                        // Set key to AudioSource.name to compare name
                        AudioSource.name = Key;
                        AudioSource.clip = AudioClip;
                        AudioSource.loop = Loop;
                        AudioSource.Play();
                    }
                    break;
                case AudioControlMethod.Stop:
                    AudioSource.Stop();
                    break;
                case AudioControlMethod.Pause:
                    AudioSource.Pause();
                    break;
                case AudioControlMethod.UnPause:
                    AudioSource.UnPause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(AudioSourceCollector, x => AudioSource = x),
                CollectableMessageAnnotationFactory.Create(AudioControlMethodCollector, x => AudioControlMethod = x),
                CollectableMessageAnnotationFactory.Create(AudioClipCollector, x => AudioClip = x),
                CollectableMessageAnnotationFactory.Create(LoopCollector, x => Loop = x, nameof(Loop)),
                CollectableMessageAnnotationFactory.Create(RewindSameClipCollector, x => RewindSameClip = x, nameof(RewindSameClip)),
            };
    }
}
