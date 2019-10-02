using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/RawImageController", (int) ConnectorType.RawImageController)]
    public class RawImageController : ConnectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private RawImage rawImage = default;

        [ValueReceiver] public Texture Texture { get; set; }
        [ValueReceiver] public GameObject BaseGameObject
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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            RawImage.texture = Texture;
            return Observable.ReturnUnit();
        }
    }
}
