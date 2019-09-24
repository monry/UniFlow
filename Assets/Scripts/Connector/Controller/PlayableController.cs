using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/PlayableController", (int) ConnectorType.PlayableController)]
    public class PlayableController : ConnectorBase
    {
        [SerializeField] private PlayableControlMethod playableControlMethod = PlayableControlMethod.Play;
        [SerializeField] private TimelineAsset timelineAsset = default;
        [SerializeField] private PlayableDirector playableDirector = default;

        [UsedImplicitly] public PlayableControlMethod PlayableControlMethod
        {
            get => playableControlMethod;
            set => playableControlMethod = value;
        }
        [UsedImplicitly] public TimelineAsset TimelineAsset
        {
            get => timelineAsset;
            set => timelineAsset = value;
        }
        [UsedImplicitly] public PlayableDirector PlayableDirector
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            InvokePlayableDirectorMethod();
            return Observable.Return(Message.Create(this));
//            return Observable
//                .Create<IMessage>(
//                    observer =>
//                    {
//                        InvokePlayableDirectorMethod();
//                        observer.OnNext(Message.Create(this));
//                        return Disposable;
//                    }
//                );
        }

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

        public class Message : MessageBase<PlayableController>
        {
            public static Message Create(PlayableController sender)
            {
                return Create<Message>(ConnectorType.PlayableController, sender);
            }
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