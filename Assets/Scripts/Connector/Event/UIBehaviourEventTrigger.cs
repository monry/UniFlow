using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/UIBehaviourEventTrigger", (int) ConnectorType.UIBehaviourEventTrigger)]
    public class UIBehaviourEventTrigger : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private UIBehaviour uiBehaviour = default;
        [SerializeField] private EventTriggerType eventTriggerType = EventTriggerType.PointerClick;
        [SerializeField] private bool activateBeforeConnect = default;
        [SerializeField] private bool deactivateAfterConnect = default;

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
        private UIBehaviour UIBehaviour
        {
            get => uiBehaviour ? uiBehaviour : uiBehaviour = ((IBaseGameObjectSpecifyable) this).GetComponent<UIBehaviour>();
            set => uiBehaviour = value;
        }
        private EventTriggerType EventTriggerType => eventTriggerType;
        private bool ActivateBeforeConnect
        {
            get => activateBeforeConnect;
            set => activateBeforeConnect = value;
        }
        private bool DeactivateAfterConnect
        {
            get => deactivateAfterConnect;
            set => deactivateAfterConnect = value;
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private UIBehaviourCollector uiBehaviourCollector = new UIBehaviourCollector();
        [SerializeField] private BoolCollector activateBeforeConnectCollector = new BoolCollector();
        [SerializeField] private BoolCollector deactivateAfterConnectCollector = new BoolCollector();
        // TODO: Implement EnumCollector

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private UIBehaviourCollector UIBehaviourCollector => uiBehaviourCollector;
        private BoolCollector ActivateBeforeConnectCollector => activateBeforeConnectCollector;
        private BoolCollector DeactivateAfterConnectCollector => deactivateAfterConnectCollector;

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable
                .ReturnUnit()
                .Do(
                    _ =>
                    {
                        if (ActivateBeforeConnect && UIBehaviour is Graphic graphic)
                        {
                            graphic.raycastTarget = true;
                        }
                    }
                )
                .SelectMany(_ => OnEventTriggerAsObservable())
                .Do(
                    _ =>
                    {
                        if (DeactivateAfterConnect && UIBehaviour is Graphic graphic)
                        {
                            graphic.raycastTarget = false;
                        }
                    }
                )
                .AsMessageObservable(this);
        }

        private IObservable<BaseEventData> OnEventTriggerAsObservable()
        {
            switch (EventTriggerType)
            {
                case EventTriggerType.PointerEnter:
                    return UIBehaviour.OnPointerEnterAsObservable();
                case EventTriggerType.PointerExit:
                    return UIBehaviour.OnPointerExitAsObservable();
                case EventTriggerType.PointerDown:
                    return UIBehaviour.OnPointerDownAsObservable();
                case EventTriggerType.PointerUp:
                    return UIBehaviour.OnPointerUpAsObservable();
                case EventTriggerType.PointerClick:
                    return UIBehaviour.OnPointerClickAsObservable();
                case EventTriggerType.Drag:
                    return UIBehaviour.OnDragAsObservable();
                case EventTriggerType.Drop:
                    return UIBehaviour.OnDropAsObservable();
                case EventTriggerType.Scroll:
                    return UIBehaviour.OnScrollAsObservable();
                case EventTriggerType.UpdateSelected:
                    return UIBehaviour.OnUpdateSelectedAsObservable();
                case EventTriggerType.Select:
                    return UIBehaviour.OnSelectAsObservable();
                case EventTriggerType.Deselect:
                    return UIBehaviour.OnDeselectAsObservable();
                case EventTriggerType.Move:
                    return UIBehaviour.OnMoveAsObservable();
                case EventTriggerType.InitializePotentialDrag:
                    return UIBehaviour.OnInitializePotentialDragAsObservable();
                case EventTriggerType.BeginDrag:
                    return UIBehaviour.OnBeginDragAsObservable();
                case EventTriggerType.EndDrag:
                    return UIBehaviour.OnEndDragAsObservable();
                case EventTriggerType.Submit:
                    return UIBehaviour.OnSubmitAsObservable();
                case EventTriggerType.Cancel:
                    return UIBehaviour.OnCancelAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<GameObject>.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotation<string>.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotation<UIBehaviour>.Create(UIBehaviourCollector, x => UIBehaviour = x),
                CollectableMessageAnnotation<bool>.Create(ActivateBeforeConnectCollector, x => ActivateBeforeConnect = x, nameof(ActivateBeforeConnect)),
                CollectableMessageAnnotation<bool>.Create(DeactivateAfterConnectCollector, x => DeactivateAfterConnect = x, nameof(DeactivateAfterConnect)),
            };
    }
}
