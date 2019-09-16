using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Timer", (int) ConnectorType.Timer)]
    public class Timer : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        [SerializeField] private bool ignoreTimeScale = default;

        [UsedImplicitly] public float Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        [UsedImplicitly] private bool IgnoreTimeScale
        {
            get => ignoreTimeScale;
            set => ignoreTimeScale = value;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Timer(TimeSpan.FromSeconds(Seconds), IgnoreTimeScale ? Scheduler.MainThreadIgnoreTimeScale : Scheduler.MainThread)
                .Select(_ => Message.Create(this));
        }

        public class Message : MessageBase<Timer>
        {
            public static Message Create(Timer sender)
            {
                return Create<Message>(ConnectorType.Timer, sender);
            }
        }
    }
}