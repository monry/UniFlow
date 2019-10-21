using System;
using System.Collections.Generic;
using UniFlow;
using UniFlow.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PretendLand.Edwin.Scripts.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/GaugeImageRenderer", (int) ConnectorType.GaugeImageRenderer)]
    public class GaugeImageRenderer : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private Image image = default;

        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            private set => baseGameObject = value;
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

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private ImageCollector imageCollector = new ImageCollector();
        [SerializeField] private FloatObservableCollector floatObservableCollector = new FloatObservableCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private ImageCollector ImageCollector => imageCollector;
        private FloatObservableCollector FloatObservableCollector => floatObservableCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(ImageCollector, x => Image = x, nameof(Image)),
                CollectableMessageAnnotationFactory.Create(FloatObservableCollector, observable => observable.Subscribe(x => Image.fillAmount = x), "FillAmountObservable"),
            };
    }
}
