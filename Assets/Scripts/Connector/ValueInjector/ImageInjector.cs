using UniFlow.Attribute;
using UniFlow.Connector.ValueInjector;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/ValueInjector/ImageInjector", (int) ConnectorType.ValueInjectorImage)]
    public class ImageInjector : InjectorBase
    {
        [SerializeField] private Image image = default;
        [SerializeField] private Sprite sprite = default;

        [ValueReceiver] public Image Image
        {
            get =>
                image != default
                    ? image
                    : image =
                        BaseGameObject.GetComponent<Image>() != default
                            ? BaseGameObject.GetComponent<Image>()
                            : BaseGameObject.AddComponent<Image>();
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
