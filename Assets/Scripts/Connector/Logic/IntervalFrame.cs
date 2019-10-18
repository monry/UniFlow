using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/IntervalFrame", (int) ConnectorType.IntervalFrame)]
    public class IntervalFrame : ConnectorBase, IMessageCollectable
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
                .IntervalFrame(Frames, FrameCountType)
                .AsMessageObservable(this, MessageParameterKey);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<int>.Create(FramesCollector, x => Frames = x, nameof(Frames)),
                CollectableMessageAnnotation<FrameCountType>.Create(FrameCountTypeCollector, x => FrameCountType = x, nameof(FrameCountType)),
            };
    }
}
