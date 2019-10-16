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

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private AudioSourceCollector audioSourceCollector = default;
        [SerializeField] private AudioClipCollector audioClipCollector = default;

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private AudioClipCollector AudioClipCollector => audioClipCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            InvokeAudioSourceMethod();
            return ObservableFactory.ReturnMessage(this);
        }

        private void InvokeAudioSourceMethod()
        {
            if (AudioClip != default)
            {
                AudioSource.clip = AudioClip;
            }

            switch (AudioControlMethod)
            {
                case AudioControlMethod.Play:
                    AudioSource.Play();
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
                new CollectableMessageAnnotation<GameObject>(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                new CollectableMessageAnnotation<string>(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                new CollectableMessageAnnotation<AudioSource>(AudioSourceCollector, x => AudioSource = x),
                new CollectableMessageAnnotation<AudioClip>(AudioClipCollector, x => AudioClip = x),
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
