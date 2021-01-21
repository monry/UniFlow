using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UniFlow.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/NumberImageRenderer", (int) ConnectorType.Custom)]
    public class NumberImageRenderer : ConnectorBase, INumberRenderer, IBaseGameObjectSpecifyable, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private int current = default;
        [SerializeField] private GameObject digitPrefab = default;
        [SerializeField] private float delaySeconds = default;
        [SerializeField] private int delayFrames = default;
        [SerializeField] private int minimumValue = 0;
        [SerializeField] private int maximumValue = int.MaxValue;
        [SerializeField] private List<GameObject> digitGameObjects = default;
        [SerializeField] private List<Sprite> numberImages = default;

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
        private int Current
        {
            get => current;
            set => current = value;
        }
        private GameObject DigitPrefab
        {
            get => digitPrefab;
            set => digitPrefab = value;
        }
        private float DelaySeconds
        {
            get => delaySeconds;
            set => delaySeconds = value;
        }
        private int DelayFrames
        {
            get => delayFrames;
            set => delayFrames = value;
        }
        private int MinimumValue
        {
            get => minimumValue;
            set => minimumValue = value;
        }
        private int MaximumValue
        {
            get => maximumValue;
            set => maximumValue = value;
        }
        private IList<GameObject> DigitGameObjects => digitGameObjects;
        private IList<Sprite> NumberImages => numberImages;

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private IntCollector currentCollector = new IntCollector();
        [SerializeField] private GameObjectCollector digitPrefabCollector = new GameObjectCollector();
        [SerializeField] private FloatCollector delaySecondsCollector = new FloatCollector();
        [SerializeField] private IntCollector delayFramesCollector = new IntCollector();
        [SerializeField] private IntCollector minimumValueCollector = new IntCollector();
        [SerializeField] private IntCollector maximumValueCollector = new IntCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private IntCollector CurrentCollector => currentCollector;
        private GameObjectCollector DigitPrefabCollector => digitPrefabCollector;
        private FloatCollector DelaySecondsCollector => delaySecondsCollector;
        private IntCollector DelayFramesCollector => delayFramesCollector;
        private IntCollector MinimumValueCollector => minimumValueCollector;
        private IntCollector MaximumValueCollector => maximumValueCollector;

        private ISubject<int> OnRenderSubject { get; } = new Subject<int>();

        public override IObservable<Message> OnConnectAsObservable()
        {
            ((INumberRenderer) this).Render(Mathf.Clamp(Current, MinimumValue, MaximumValue));
            return OnRenderSubject.AsMessageObservable(this, "Current");
        }

        void INumberRenderer.Render(int number)
        {
            Assert.AreEqual(10, NumberImages.Count);

            RenderAsync(number).Forget();
        }

        private async UniTask RenderAsync(int number)
        {
            var splittedDigits = SplitDigits(number).Reverse().ToList();

            if (DigitPrefab == default && DigitGameObjects.Any())
            {
                DigitPrefab = DigitGameObjects[0];
            }

            if (DelaySeconds > 0.0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(DelaySeconds));
            }

            if (DelayFrames > 0)
            {
                await UniTask.DelayFrame(DelayFrames);
            }

            while (splittedDigits.Count > DigitGameObjects.Count)
            {
                DigitGameObjects.Add(Instantiate(DigitPrefab, this.DeterminateTransform()));
            }

            foreach (var (n, d) in splittedDigits.Select((x, d) => (number: x, digit: d)))
            {
                DigitGameObjects[d].GetOrAddComponent<Image>().sprite = NumberImages[n];
            }

            OnRenderSubject.OnNext(number);
        }

        private static IEnumerable<int> SplitDigits(int number)
        {
            IList<int> result = new List<int>();
            do
            {
                result.Add(number % 10);
                number /= 10;
            } while (number > 0);

            return result;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(CurrentCollector, x => Current = x, nameof(Current)),
                CollectableMessageAnnotationFactory.Create(DigitPrefabCollector, x => DigitPrefab = x, nameof(DigitPrefab)),
                CollectableMessageAnnotationFactory.Create(DelaySecondsCollector, x => DelaySeconds = x, nameof(DelaySeconds)),
                CollectableMessageAnnotationFactory.Create(DelayFramesCollector, x => DelayFrames = x, nameof(DelayFrames)),
                CollectableMessageAnnotationFactory.Create(MinimumValueCollector, x => MinimumValue = x, nameof(MinimumValue)),
                CollectableMessageAnnotationFactory.Create(MaximumValueCollector, x => MaximumValue = x, nameof(MaximumValue)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create<int>(null, "Current"),
            };
    }

    public interface INumberRenderer
    {
        void Render(int number);
    }
}
