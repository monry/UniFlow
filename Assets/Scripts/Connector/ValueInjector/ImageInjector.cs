using System.Collections.Generic;
using UniFlow.Attribute;
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

        [ValueReceiver] public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public Image Image
        {
            get => image != default ? image : image = this.GetOrAddComponent<Image>();
            set => image = value;
        }
        [ValueReceiver] public Sprite Sprite
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
                new CollectableMessageAnnotation<GameObject>(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                new CollectableMessageAnnotation<string>(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                new CollectableMessageAnnotation<Image>(ImageCollector, x => Image = x, nameof(Image)),
                new CollectableMessageAnnotation<Sprite>(SpriteCollector, x => Sprite = x, nameof(Sprite)),
            };
    }
}
