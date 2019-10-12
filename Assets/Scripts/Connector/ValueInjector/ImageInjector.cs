using UniFlow.Attribute;
using UniFlow.Connector.ValueInjector;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/ValueInjector/ImageInjector", (int) ConnectorType.ValueInjectorImage)]
    public class ImageInjector : InjectorBase, IBaseGameObjectSpecifyable
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

        protected override void Inject()
        {
            Image.sprite = Sprite;
        }
    }
}
