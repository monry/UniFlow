using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/TimerFrame", (int) ConnectorType.TimerFrame)]
    public class TimerFrame : ConnectorBase, IMessageCollectable
    {
        private const string MessageParameterKey = "Count";

        [SerializeField] private int frames = default;
        [SerializeField] private FrameCountType frameCountType = FrameCountType.Update;

        [UsedImplicitly] public int Frames
        {
            get => frames;
            set => frames = value;
        }
        [UsedImplicitly] private FrameCountType FrameCountType
        {
            get => frameCountType;
            set => frameCountType = value;
        }

        [SerializeField] private IntCollector framesCollector = new IntCollector();
        [SerializeField] private FrameCountTypeCollector frameCountTypeCollector = new FrameCountTypeCollector();

        private IntCollector FramesCollector => framesCollector;
        private FrameCountTypeCollector FrameCountTypeCollector => frameCountTypeCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable
                .TimerFrame(Frames, FrameCountType)
                .AsMessageObservable(this, MessageParameterKey);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<int>.Create(FramesCollector, x => Frames = x, nameof(Frames)),
                CollectableMessageAnnotation<FrameCountType>.Create(FrameCountTypeCollector, x => FrameCountType = x, nameof(FrameCountType)),
            };
    }

    [Serializable]
    public class FrameCountTypeCollector : ValueCollectorBase<FrameCountType>
    {
    }
}
