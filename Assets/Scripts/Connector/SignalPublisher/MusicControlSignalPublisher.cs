using System.Collections.Generic;
using UniFlow.Connector.Controller;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/MusicControlSignalPublisher", (int) ConnectorType.MusicControlSignalPublisher)]
    public class MusicControlSignalPublisher : SignalPublisherBase<MusicControlSignal>, IMessageCollectable
    {
        [SerializeField] private AudioControlMethod audioControlMethod = default;
        [SerializeField] private AudioClip audioClip = default;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool rewindSameClip = false;

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

        [SerializeField] private AudioControlMethodCollector audioControlMethodCollector = new AudioControlMethodCollector();
        [SerializeField] private AudioClipCollector audioClipCollector = new AudioClipCollector();
        [SerializeField] private BoolCollector loopCollector = default;
        [SerializeField] private BoolCollector rewindSameClipCollector = new BoolCollector();

        private AudioControlMethodCollector AudioControlMethodCollector => audioControlMethodCollector;
        private AudioClipCollector AudioClipCollector => audioClipCollector;
        private BoolCollector LoopCollector => loopCollector;
        private BoolCollector RewindSameClipCollector => rewindSameClipCollector;

        protected override MusicControlSignal GetSignal() =>
            MusicControlSignal.Create(AudioControlMethod, AudioClip, Loop, RewindSameClip);

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(AudioControlMethodCollector, x => AudioControlMethod = x, nameof(AudioControlMethod)),
                CollectableMessageAnnotationFactory.Create(AudioClipCollector, x => AudioClip = x, nameof(AudioClip)),
                CollectableMessageAnnotationFactory.Create(LoopCollector, x => Loop = x, nameof(Loop)),
                CollectableMessageAnnotationFactory.Create(RewindSameClipCollector, x => RewindSameClip = x, nameof(RewindSameClip)),
            };
    }
}
