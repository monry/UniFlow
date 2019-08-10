using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/UIBehaviourEventTrigger", 101)]
    public class UIBehaviourEventTrigger : ConnectorBase
    {
        [SerializeField] private EventTriggerType eventTriggerType = default;
        private EventTriggerType EventTriggerType => eventTriggerType;

        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<UIBehaviour>()")]
        private UIBehaviour uiBehaviour = default;
        private UIBehaviour UIBehaviour => uiBehaviour ? uiBehaviour : uiBehaviour = GetComponent<UIBehaviour>();

        public override IObservable<EventMessage> OnConnectAsObservable() =>
            OnEventTriggerAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.UIBehaviourEventTrigger, UIBehaviour, x));

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