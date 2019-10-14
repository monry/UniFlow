using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/ParticleEvent", (int) ConnectorType.ParticleEvent)]
    public class ParticleEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private ParticleEventType particleEventType = ParticleEventType.ParticleCollision;

        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public ParticleEventType ParticleEventType
        {
            get => particleEventType;
            set => particleEventType = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(this.CreateMessage);
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
    }

    public enum ParticleEventType
    {
        ParticleCollision,
        ParticleTrigger,
//        ParticleSystemStopped,
    }
}
