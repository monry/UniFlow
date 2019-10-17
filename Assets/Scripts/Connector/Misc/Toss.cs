using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Misc
{
    [AddComponentMenu("UniFlow/Misc/Toss", (int) ConnectorType.Toss)]
    public class Toss : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private List<GameObject> targets = default;
        private IList<GameObject> Targets => targets;

        private GameObject TargetGameObject
        {
            set => Targets.Add(value);
        }

        [SerializeField] private GameObjectCollector targetGameObjectCollector = new GameObjectCollector();
        private GameObjectCollector TargetGameObjectCollector => targetGameObjectCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            foreach (var target in Targets)
            {
                if (target != default && target.activeSelf)
                {
                    (target.GetComponent<Receive>() as IConnector)?.Connect(ObservableFactory.ReturnMessage(this));
                }
            }
            return ObservableFactory.ReturnMessage(this);
        }

        public IEnumerable<ICollectableMessageAnnotation> GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<GameObject>.Create(TargetGameObjectCollector, x => TargetGameObject = x, nameof(TargetGameObject)),
            };
    }
}
