using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/UIBehaviourEventTrigger", (int) ConnectorType.UIBehaviourEventTrigger)]
    public class UIBehaviourEventTrigger : ConnectorBase
    {
        [SerializeField] private EventTriggerType eventTriggerType = EventTriggerType.PointerClick;
        [SerializeField] private UIBehaviour uiBehaviour = default;
        [SerializeField] private bool activateBeforeConnect = default;
        [SerializeField] private bool deactivateAfterConnect = default;

        [UsedImplicitly] public EventTriggerType EventTriggerType
        {
            get => eventTriggerType;
            set => eventTriggerType = value;
        }
        [UsedImplicitly] public UIBehaviour UIBehaviour
        {
            get => uiBehaviour ? uiBehaviour : uiBehaviour = GetComponent<UIBehaviour>();
            set => uiBehaviour = value;
        }
        [UsedImplicitly] public bool ActivateBeforeConnect
        {
            get => activateBeforeConnect;
            set => activateBeforeConnect = value;
        }
        [UsedImplicitly] public bool DeactivateAfterConnect
        {
            get => deactivateAfterConnect;
            set => deactivateAfterConnect = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
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
                .Select(x => Message.Create(this, x));
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

        public class Message : MessageBase<UIBehaviourEventTrigger, BaseEventData>
        {
            public static Message Create(UIBehaviourEventTrigger sender, BaseEventData baseEventData)
            {
                return Create<Message>(ConnectorType.UIBehaviourEventTrigger, sender, baseEventData);
            }
        }
    }
}