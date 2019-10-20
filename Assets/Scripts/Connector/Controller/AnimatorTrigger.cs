using System;
using System.Collections.Generic;
using UniFlow.Utility;
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

        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            private set => baseGameObject = value;
        }
        public string TransformPath
        {
            get => transformPath;
            private set => transformPath = value;
        }
        private Animator Animator
        {
            get => animator ? animator : animator = this.GetOrAddComponent<Animator>();
            set => animator = value;
        }
        private string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private AnimatorCollector animatorCollector = new AnimatorCollector();
        [SerializeField] private StringCollector triggerNameCollector = new StringCollector();

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
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(AnimatorCollector, x => Animator = x),
                CollectableMessageAnnotationFactory.Create(TriggerNameCollector, x => TriggerName = x, nameof(TriggerName)),
            };
    }
}
