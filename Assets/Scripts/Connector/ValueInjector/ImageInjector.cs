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
        [SerializeField] private Color color = default;
        [SerializeField] private Sprite sprite = default;
        [SerializeField] private bool invokeSetNativeSize = default;

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
            set
            {
                sprite = value;
                HasInjectSprite = true;
            }
        }
        private Color Color
        {
            get => color;
            set
            {
                color = value;
                HasInjectColor = true;
            }
        }
        private bool InvokeSetNativeSize
        {
            get => invokeSetNativeSize;
            set => invokeSetNativeSize = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private ImageCollector imageCollector = new ImageCollector();
        [SerializeField] private SpriteCollector spriteCollector = new SpriteCollector();
        [SerializeField] private ColorCollector colorCollector = new ColorCollector();
        [SerializeField] private BoolCollector invokeSetNativeSizeCollector = new BoolCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private ImageCollector ImageCollector => imageCollector;
        private SpriteCollector SpriteCollector => spriteCollector;
        private ColorCollector ColorCollector => colorCollector;
        private BoolCollector InvokeSetNativeSizeCollector => invokeSetNativeSizeCollector;

        private bool HasInjectColor { get; set; }
        private bool HasInjectSprite { get; set; }

        protected override void Inject()
        {
            if (HasInjectColor)
            {
                Image.color = Color;
            }

            if (HasInjectSprite)
            {
                Image.sprite = Sprite;
            }

            if (InvokeSetNativeSize)
            {
                Image.SetNativeSize();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(ImageCollector, x => Image = x, nameof(Image)),
                CollectableMessageAnnotationFactory.Create(SpriteCollector, x => Sprite = x, nameof(Sprite)),
                CollectableMessageAnnotationFactory.Create(ColorCollector, x => Color = x, nameof(Color)),
                CollectableMessageAnnotationFactory.Create(InvokeSetNativeSizeCollector, x => InvokeSetNativeSize = x, nameof(InvokeSetNativeSize)),
            };
    }
}
