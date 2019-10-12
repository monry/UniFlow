using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/UIBehaviourEventTrigger", (int) ConnectorType.UIBehaviourEventTrigger)]
    public class UIBehaviourEventTrigger : ConnectorBase, IBaseGameObjectSpecifyable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private UIBehaviour uiBehaviour = default;
        [SerializeField] private EventTriggerType eventTriggerType = EventTriggerType.PointerClick;
        [SerializeField] private bool activateBeforeConnect = default;
        [SerializeField] private bool deactivateAfterConnect = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public UIBehaviour UIBehaviour
        {
            get => uiBehaviour ? uiBehaviour : uiBehaviour = ((IBaseGameObjectSpecifyable) this).GetComponent<UIBehaviour>();
            set => uiBehaviour = value;
        }
        [UsedImplicitly] public EventTriggerType EventTriggerType
        {
            get => eventTriggerType;
            set => eventTriggerType = value;
        }
        [ValueReceiver] public bool ActivateBeforeConnect
        {
            get => activateBeforeConnect;
            set => activateBeforeConnect = value;
        }
        [ValueReceiver] public bool DeactivateAfterConnect
        {
            get => deactivateAfterConnect;
            set => deactivateAfterConnect = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
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
                .AsUnitObservable();
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
    }
}
