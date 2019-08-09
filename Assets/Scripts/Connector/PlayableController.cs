using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PlayableController", 305)]
    public class PlayableController : EventPublisher
    {
        [SerializeField] private PlayableControlMethod playableControlMethod = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<PlayableDirector>()")]
        private PlayableDirector playableDirector = default;
        [SerializeField] private TimelineAsset timelineAsset = default;

        private PlayableControlMethod PlayableControlMethod => playableControlMethod;
        private PlayableDirector PlayableDirector
        {
            get =>
                playableDirector != default
                    ? playableDirector
                    : playableDirector =
                        GetComponent<PlayableDirector>() != default
                            ? GetComponent<PlayableDirector>()
                            : gameObject.AddComponent<PlayableDirector>();
            [UsedImplicitly]
            set => playableDirector = value;
        }
        private TimelineAsset TimelineAsset
        {
            get => timelineAsset;
            [UsedImplicitly]
            set => timelineAsset = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        InvokePlayableDirectorMethod();
                        observer.OnNext(EventMessage.Create(EventType.PlayableController, PlayableDirector, PlayableControllerEventData.Create(PlayableControlMethod)));
                        return Disposable;
                    }
                );

        private void InvokePlayableDirectorMethod()
        {
            if (TimelineAsset != default)
            {
                PlayableDirector.playableAsset = TimelineAsset;
            }

            switch (PlayableControlMethod)
            {
                case PlayableControlMethod.Play:
                    PlayableDirector.Play();
                    break;
                case PlayableControlMethod.Stop:
                    PlayableDirector.Stop();
                    break;
                case PlayableControlMethod.Pause:
                    PlayableDirector.Pause();
                    break;
                case PlayableControlMethod.Resume:
                    PlayableDirector.Resume();
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

    public enum PlayableControlMethod
    {
        Play,
        Stop,
        Pause,
        Resume,
    }
}