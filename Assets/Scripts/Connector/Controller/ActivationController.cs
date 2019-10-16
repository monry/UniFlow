using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Utility;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/ActivationController", (int) ConnectorType.LoadScene)]
    public class ActivationController : ConnectorBase,
        IMessageCollectable
    {
        [SerializeField] private List<GameObject> targetGameObjects = default;
        [SerializeField] private List<Behaviour> targetBehaviours = default;
        [SerializeField] private bool activated = default;

        [UsedImplicitly] public IList<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }
        [UsedImplicitly] public IList<Behaviour> Behaviours
        {
            get => targetBehaviours;
            set => targetBehaviours = value.ToList();
        }
        [ValueReceiver] public bool Activated
        {
            get => activated;
            set => activated = value;
        }
        [ValueReceiver] public GameObject TargetGameObject
        {
            get => null;
            set => GameObjects.Add(value);
        }
        [ValueReceiver] public Behaviour TargetBehaviour
        {
            get => null;
            set => Behaviours.Add(value);
        }

        [SerializeField] private BoolCollector activatedCollector = default;
        [SerializeField] private GameObjectCollector targetGameObjectCollector = default;
        [SerializeField] private BehaviourCollector targetBehaviourCollector = default;

        private BoolCollector ActivatedCollector => activatedCollector;
        private GameObjectCollector TargetGameObjectCollector => targetGameObjectCollector;
        private BehaviourCollector TargetBehaviourCollector => targetBehaviourCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var gameObjects = GameObjects.Where(x => x.activeSelf != Activated).ToList();
            gameObjects.ForEach(x => x.SetActive(Activated));
            var monoBehaviours = Behaviours.Where(x => x.enabled != Activated).ToList();
            monoBehaviours.ForEach(x => x.enabled = Activated);

            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<bool>(ActivatedCollector, x => Activated = x, nameof(Activated)),
                new CollectableMessageAnnotation<GameObject>(TargetGameObjectCollector, x => TargetGameObject = x, nameof(TargetGameObject)),
                new CollectableMessageAnnotation<Behaviour>(TargetBehaviourCollector, x => TargetBehaviour = x, nameof(TargetBehaviour)),
            };
    }
}
