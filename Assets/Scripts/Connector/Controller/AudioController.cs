using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/AudioController", (int) ConnectorType.AudioController)]
    public class AudioController : ConnectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private AudioControlMethod audioControlMethod = AudioControlMethod.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by AudioSource.clip")]
        private AudioClip audioClip = default;
        [SerializeField] private bool loop = default;
        [SerializeField] private bool ignorePlayWhenPlaying = default;

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
        private AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = this.GetOrAddComponent<AudioSource>();
            set => audioSource = value;
        }
        private AudioControlMethod AudioControlMethod => audioControlMethod;
        private AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }
        private bool Loop
        {
            get => loop;
            set => loop = value;
        }
        private bool IgnorePlayWhenPlaying
        {
            get => ignorePlayWhenPlaying;
            set => ignorePlayWhenPlaying = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private AudioSourceCollector audioSourceCollector = new AudioSourceCollector();
        [SerializeField] private AudioClipCollector audioClipCollector = new AudioClipCollector();
        [SerializeField] private BoolCollector loopCollector = default;
        [SerializeField] private BoolCollector ignorePlayWhenPlayingCollector = new BoolCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private AudioClipCollector AudioClipCollector => audioClipCollector;
        private BoolCollector LoopCollector => loopCollector;
        private BoolCollector IgnorePlayWhenPlayingCollector => ignorePlayWhenPlayingCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            InvokeAudioSourceMethod();
            return ObservableFactory.ReturnMessage(this);
        }

        private void InvokeAudioSourceMethod()
        {
            if (AudioClip != default)
            {
                AudioSource.playOnAwake = false;
                AudioSource.loop = Loop;
                AudioSource.clip = AudioClip;
            }

            switch (AudioControlMethod)
            {
                case AudioControlMethod.Play:
                    if (!IgnorePlayWhenPlaying || !AudioSource.isPlaying)
                    {
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
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<GameObject>.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotation<string>.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotation<AudioSource>.Create(AudioSourceCollector, x => AudioSource = x),
                CollectableMessageAnnotation<AudioClip>.Create(AudioClipCollector, x => AudioClip = x),
                CollectableMessageAnnotation<bool>.Create(LoopCollector, x => Loop = x, nameof(Loop)),
                CollectableMessageAnnotation<bool>.Create(IgnorePlayWhenPlayingCollector, x => IgnorePlayWhenPlaying = x, nameof(IgnorePlayWhenPlaying)),
            };
    }

    public enum AudioControlMethod
    {
        Play,
        Stop,
        Pause,
        UnPause,
    }
}
