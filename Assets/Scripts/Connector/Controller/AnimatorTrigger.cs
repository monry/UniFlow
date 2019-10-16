using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/AnimatorTrigger", (int) ConnectorType.AnimatorTrigger)]
    public class AnimatorTrigger : ConnectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [SerializeField] private string triggerName = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public Animator Animator
        {
            get => animator ? animator : animator = this.GetOrAddComponent<Animator>();
            set => animator = value;
        }
        [ValueReceiver] public string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private AnimatorCollector animatorCollector = default;
        [SerializeField] private StringCollector triggerNameCollector = default;

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private AnimatorCollector AnimatorCollector => animatorCollector;
        private StringCollector TriggerNameCollector => triggerNameCollector;

        private int TriggerId => Animator.StringToHash(TriggerName);

        public override IObservable<Message> OnConnectAsObservable()
        {
            Animator.SetTrigger(TriggerId);
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<GameObject>(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                new CollectableMessageAnnotation<string>(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                new CollectableMessageAnnotation<Animator>(AnimatorCollector, x => Animator = x),
                new CollectableMessageAnnotation<string>(TriggerNameCollector, x => TriggerName = x, nameof(TriggerName)),
            };
    }
}
