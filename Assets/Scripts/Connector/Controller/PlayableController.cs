using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/PlayableController", (int) ConnectorType.PlayableController)]
    public class PlayableController : ConnectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private PlayableControlMethod playableControlMethod = PlayableControlMethod.Play;
        [SerializeField] private TimelineAsset timelineAsset = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public PlayableDirector PlayableDirector
        {
            get =>
                playableDirector != default
                    ? playableDirector
                    : playableDirector =
                        BaseGameObject.GetComponent<PlayableDirector>() != default
                            ? BaseGameObject.GetComponent<PlayableDirector>()
                            : BaseGameObject.AddComponent<PlayableDirector>();
            set => playableDirector = value;
        }
        [UsedImplicitly] public PlayableControlMethod PlayableControlMethod
        {
            get => playableControlMethod;
            set => playableControlMethod = value;
        }
        [ValueReceiver] public TimelineAsset TimelineAsset
        {
            get => timelineAsset;
            set => timelineAsset = value;
        }

        private void Awake()
        {
            if (PlayableDirector != default && TimelineAsset != default)
            {
                PlayableDirector.playableAsset = TimelineAsset;
            }
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            InvokePlayableDirectorMethod();
            return Observable.ReturnUnit();
        }

        private void InvokePlayableDirectorMethod()
        {

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
    }

    public enum PlayableControlMethod
    {
        Play,
        Stop,
        Pause,
        Resume,
    }
}
