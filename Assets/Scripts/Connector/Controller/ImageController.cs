using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/ImageController", (int) ConnectorType.ImageController)]
    public class ImageController : ConnectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private Image image = default;

        [ValueReceiver] public Sprite Sprite { get; set; }
        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Image.sprite = Sprite;
            return Observable.ReturnUnit();
        }
    }
}
