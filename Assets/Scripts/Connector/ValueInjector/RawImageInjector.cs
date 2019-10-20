using System.Collections.Generic;
using UniFlow.Connector.ValueInjector;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/ValueInjector/RawImageInjector", (int) ConnectorType.ValueInjectorRawImage)]
    public class RawImageInjector : InjectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private RawImage rawImage = default;
        [SerializeField] private Texture texture = default;

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
        private RawImage RawImage
        {
            get => rawImage != default ? rawImage : rawImage = this.GetOrAddComponent<RawImage>();
            set => rawImage = value;
        }
        private Texture Texture
        {
            get => texture;
            set => texture = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private RawImageCollector rawImageCollector = new RawImageCollector();
        [SerializeField] private TextureCollector textureCollector = new TextureCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector ?? (baseGameObjectCollector = new GameObjectCollector {TargetConnector = this});
        private StringCollector TransformPathCollector => transformPathCollector ?? (transformPathCollector = new StringCollector {TargetConnector = this});
        private RawImageCollector RawImageCollector => rawImageCollector ?? (rawImageCollector = new RawImageCollector {TargetConnector = this});
        private TextureCollector TextureCollector => textureCollector ?? (textureCollector = new TextureCollector {TargetConnector = this});

        protected override void Inject()
        {
            RawImage.texture = Texture;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(RawImageCollector, x => RawImage = x, nameof(RawImage)),
                CollectableMessageAnnotationFactory.Create(TextureCollector, x => Texture = x, nameof(Texture)),
            };
    }
}
