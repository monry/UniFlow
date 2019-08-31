using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/UIBehaviourEventTrigger", (int) ConnectorType.UIBehaviourEventTrigger)]
    public class UIBehaviourEventTrigger : ConnectorBase
    {
        [SerializeField] private EventTriggerType eventTriggerType = (EventTriggerType) (-1);
        [SerializeField] private UIBehaviour uiBehaviour = default;

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

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventTriggerAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.UIBehaviourEventTrigger, UIBehaviour, x));
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
    }
}