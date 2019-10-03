using UniFlow.Attribute;
using UniFlow.Connector.ValueInjector;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/ValueInjector/RawImageInjector", (int) ConnectorType.ValueInjectorRawImage)]
    public class RawImageInjector : InjectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private RawImage rawImage = default;
        [SerializeField] private Texture texture = default;

        [ValueReceiver] public override GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public RawImage RawImage
        {
            get =>
                rawImage != default
                    ? rawImage
                    : rawImage =
                        BaseGameObject.GetComponent<RawImage>() != default
                            ? BaseGameObject.GetComponent<RawImage>()
                            : BaseGameObject.AddComponent<RawImage>();
            set => rawImage = value;
        }
        [ValueReceiver] public Texture Texture
        {
            get => texture;
            set => texture = value;
        }

        protected override void Inject()
        {
            RawImage.texture = Texture;
        }
    }
}