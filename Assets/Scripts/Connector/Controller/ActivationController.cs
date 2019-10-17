using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Utility;
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
        public GameObject TargetGameObject
        {
            set => GameObjects.Add(value);
        }
        private Behaviour TargetBehaviour
        {
            set => Behaviours.Add(value);
        }
        private bool Activated
        {
            get => activated;
            set => activated = value;
        }

        [SerializeField] private BoolCollector activatedCollector = new BoolCollector();
        [SerializeField] private GameObjectCollector targetGameObjectCollector = new GameObjectCollector();
        [SerializeField] private BehaviourCollector targetBehaviourCollector = new BehaviourCollector();

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
                CollectableMessageAnnotation<bool>.Create(ActivatedCollector, x => Activated = x, nameof(Activated)),
                CollectableMessageAnnotation<GameObject>.Create(TargetGameObjectCollector, x => TargetGameObject = x, nameof(TargetGameObject)),
                CollectableMessageAnnotation<Behaviour>.Create(TargetBehaviourCollector, x => TargetBehaviour = x, nameof(TargetBehaviour)),
            };
    }
}
