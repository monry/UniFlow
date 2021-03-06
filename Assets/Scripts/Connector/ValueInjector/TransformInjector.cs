using System.Collections.Generic;
using UniFlow;
using UniFlow.Connector.ValueInjector;
using UnityEngine;

namespace CAFUSample.Scripts.Connector.ValueInjector
{
    [AddComponentMenu("UniFlow/ValueInjector/TransformInjector", (int) ConnectorType.ValueInjectorTransform)]
    public class TransformInjector : InjectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private bool actAsLocal = true;
        [SerializeField] private Vector3 position = default;
        [SerializeField] private Quaternion rotation = default;
        [SerializeField] private Vector3 rotationEuler = default;
        [SerializeField] private Vector3 scale = default;

        public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        public string TransformPath
        {
            get => transformPath;
            private set => transformPath = value;
        }
        private bool ActAsLocal => actAsLocal;
        private Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                HasInjectPosition = true;
            }
        }
        private Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                HasInjectRotation = true;
            }
        }
        private Vector3 RotationEuler
        {
            get => rotationEuler;
            set
            {
                rotationEuler = value;
                HasInjectRotationEuler = true;
            }
        }
        private Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                HasInjectScale = true;
            }
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private Vector3Collector positionCollector = new Vector3Collector();
        [SerializeField] private QuaternionCollector rotationCollector = new QuaternionCollector();
        [SerializeField] private Vector3Collector rotationEulerCollector = new Vector3Collector();
        [SerializeField] private Vector3Collector scaleCollector = new Vector3Collector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private Vector3Collector PositionCollector => positionCollector;
        private QuaternionCollector RotationCollector => rotationCollector;
        private Vector3Collector RotationEulerCollector => rotationEulerCollector;
        private Vector3Collector ScaleCollector => scaleCollector;

        private bool HasInjectPosition { get; set; }
        private bool HasInjectRotation { get; set; }
        private bool HasInjectRotationEuler { get; set; }
        private bool HasInjectScale { get; set; }

        protected override void Inject()
        {
            var targetTransform = BaseGameObject.transform;
            if (HasInjectPosition)
            {
                if (ActAsLocal)
                {
                    targetTransform.localPosition = Position;
                }
                else
                {
                    targetTransform.position = Position;
                }
            }

            if (HasInjectRotationEuler)
            {
                if (ActAsLocal)
                {
                    targetTransform.localRotation = Quaternion.Euler(RotationEuler);
                }
                else
                {
                    targetTransform.rotation = Quaternion.Euler(RotationEuler);
                }
            }

            if (HasInjectRotation)
            {
                if (ActAsLocal)
                {
                    targetTransform.localRotation = Rotation;
                }
                else
                {
                    targetTransform.rotation = Rotation;
                }
            }

            // ReSharper disable once InvertIf
            if (HasInjectScale)
            {
                if (ActAsLocal)
                {
                    targetTransform.localScale = Scale;
                }
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(PositionCollector, x => Position = x, nameof(Position)),
                CollectableMessageAnnotationFactory.Create(RotationCollector, x => Rotation = x, nameof(Rotation)),
                CollectableMessageAnnotationFactory.Create(RotationEulerCollector, x => RotationEuler = x, nameof(RotationEuler)),
                CollectableMessageAnnotationFactory.Create(ScaleCollector, x => Scale = x, nameof(Scale)),
            };
    }
}
