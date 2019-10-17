using System.Collections.Generic;
using UniFlow.Connector.ValueInjector;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/ValueInjector/ImageInjector", (int) ConnectorType.ValueInjectorImage)]
    public class ImageInjector : InjectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private Image image = default;
        [SerializeField] private Sprite sprite = default;

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
        private Image Image
        {
            get => image != default ? image : image = this.GetOrAddComponent<Image>();
            set => image = value;
        }
        private Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private ImageCollector imageCollector = default;
        [SerializeField] private SpriteCollector spriteCollector = default;

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private ImageCollector ImageCollector => imageCollector;
        private SpriteCollector SpriteCollector => spriteCollector;

        protected override void Inject()
        {
            Image.sprite = Sprite;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<GameObject>.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotation<string>.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotation<Image>.Create(ImageCollector, x => Image = x, nameof(Image)),
                CollectableMessageAnnotation<Sprite>.Create(SpriteCollector, x => Sprite = x, nameof(Sprite)),
            };
    }
}
