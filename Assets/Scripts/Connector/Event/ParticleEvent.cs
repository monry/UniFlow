using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/ParticleEvent", (int) ConnectorType.ParticleEvent)]
    public class ParticleEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private ParticleEventType particleEventType = ParticleEventType.ParticleCollision;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private ParticleEventType ParticleEventType => particleEventType;

        [SerializeField] private ComponentCollector componentCollector = new ComponentCollector();

        private ComponentCollector ComponentCollector => componentCollector;

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<Component>.Create(ComponentCollector, x => Component = x),
            };
    }

    public enum ParticleEventType
    {
        ParticleCollision,
        ParticleTrigger,
//        ParticleSystemStopped,
    }
}
