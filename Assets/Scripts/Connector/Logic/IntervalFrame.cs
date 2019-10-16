using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/IntervalFrame", (int) ConnectorType.IntervalFrame)]
    public class IntervalFrame : ConnectorBase
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

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable
                .IntervalFrame(Frames, FrameCountType)
                .AsMessageObservable(this, MessageParameterKey);
        }
    }
}
