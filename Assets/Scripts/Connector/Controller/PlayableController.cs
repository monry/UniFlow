using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/PlayableController", (int) ConnectorType.PlayableController)]
    public class PlayableController : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private PlayableControlMethod playableControlMethod = PlayableControlMethod.Play;
        [SerializeField] private TimelineAsset timelineAsset = default;

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

        public PlayableDirector PlayableDirector
        {
            get => playableDirector != default ? playableDirector : playableDirector = this.GetOrAddComponent<PlayableDirector>();
            private set => playableDirector = value;
        }
        private PlayableControlMethod PlayableControlMethod => playableControlMethod;
        private TimelineAsset TimelineAsset
        {
            get => timelineAsset;
            set => timelineAsset = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private PlayableDirectorCollector playableDirectorCollector = new PlayableDirectorCollector();
        [SerializeField] private TimelineAssetCollector timelineAssetCollector = new TimelineAssetCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private PlayableDirectorCollector PlayableDirectorCollector => playableDirectorCollector;
        private TimelineAssetCollector TimelineAssetCollector => timelineAssetCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            PreparePlayableDirector();
            InvokePlayableDirectorMethod();
            return ObservableFactory.ReturnMessage(this);
        }

        private void PreparePlayableDirector()
        {
            if (PlayableDirector != default && TimelineAsset != default)
            {
                PlayableDirector.playableAsset = TimelineAsset;
            }
        }

        private void InvokePlayableDirectorMethod()
        {

            switch (PlayableControlMethod)
            {
                case PlayableControlMethod.Play:
                    PlayableDirector.time = 0;
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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(PlayableDirectorCollector, x => PlayableDirector = x),
                CollectableMessageAnnotationFactory.Create(TimelineAssetCollector, x => TimelineAsset = x),
            };
    }

    public enum PlayableControlMethod
    {
        Play,
        Stop,
        Pause,
        Resume,
    }
}
