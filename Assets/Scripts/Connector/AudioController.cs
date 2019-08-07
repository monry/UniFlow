using System;
using EventConnector.Message;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    public class AudioController : EventConnector
    {
        [SerializeField] private AudioControlMethod audioControlMethod = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<AudioSource>()")]
        private AudioSource audioSource = default;

        private AudioControlMethod AudioControlMethod => audioControlMethod;
        private AudioSource AudioSource => audioSource ? audioSource : audioSource = GetComponent<AudioSource>();

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> FooAsObservable() =>
            Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        InvokeAudioSourceMethod();
                        observer.OnNext(EventMessage.Create(EventType.AudioController, AudioSource, AudioControllerEventData.Create(AudioControlMethod)));
                        return Disposable;
                    }
                );

        private void InvokeAudioSourceMethod()
        {
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

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }

    public enum AudioControlMethod
    {
        Play,
        Stop,
        Pause,
        UnPause,
    }
}