using System;
using EventConnector.Message;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace EventConnector.Connector
{
    [AddComponentMenu("Event Connector/PlayableController")]
    public class PlayableController : EventConnector, IEventPublisher
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<PlayableDirector>()")]
        private PlayableDirector playableDirector = default;
        private PlayableDirector PlayableDirector => playableDirector ? playableDirector : playableDirector = GetComponent<PlayableDirector>();

        private IDisposable Disposable { get; } = new CompositeDisposable();

        IObservable<EventMessage> IEventPublisher.OnPublishAsObservable() =>
            Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        PlayableDirector.Play();
                        observer.OnNext(EventMessage.Create(EventType.PlayableController, PlayableDirector, PlayableControllerEventData.Create()));
                        return Disposable;
                    }
                );

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}