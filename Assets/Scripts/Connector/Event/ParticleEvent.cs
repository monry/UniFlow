using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/ParticleEvent", (int) ConnectorType.ParticleEvent)]
    public class ParticleEvent : ConnectorBase
    {
        [SerializeField] private ParticleEventType particleEventType = ParticleEventType.ParticleCollision;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public ParticleEventType ParticleEventType
        {
            get => particleEventType;
            set => particleEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return OnEventAsObservable()
                .Select(x => Message.Create(this));
        }

        private IObservable<GameObject> OnEventAsObservable()
        {
            switch (ParticleEventType)
            {
                case ParticleEventType.ParticleCollision:
                    return Component.OnParticleCollisionAsObservable();
                case ParticleEventType.ParticleTrigger:
                    return Component.OnParticleTriggerAsObservable().Select(_ => Component.gameObject);
// TODO: Uncomment when the ObservableParticleSystemStoppedTrigger is implemented
//                case ParticleEventType.ParticleSystemStopped:
//                    return Component.OnParticleSystemStoppedAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<ParticleEvent>
        {
            public static Message Create(ParticleEvent sender)
            {
                return Create<Message>(ConnectorType.ParticleEvent, sender);
            }
        }
    }

    public enum ParticleEventType
    {
        ParticleCollision,
        ParticleTrigger,
//        ParticleSystemStopped,
    }
}