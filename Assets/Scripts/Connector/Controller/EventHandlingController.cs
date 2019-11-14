using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KeyEventHandler;
using UniFlow.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/EventHandlingController", (int) ConnectorType.EventHandlingController)]
    public class EventHandlingController : ConnectorBase
    {
        [SerializeField] private EventHandlingControlMethod eventHandlingControlMethod = EventHandlingControlMethod.Activate;
        [SerializeField] private List<BaseRaycaster> raycasters = default;
        [SerializeField] private List<KeyEventLayer> keyEventLayers = default;
        [SerializeField] private List<Graphic> graphics = default;
        [SerializeField] private List<Collider> colliders = default;
        [SerializeField] private List<Collider2D> collider2Ds = default;
        [SerializeField] private List<CanvasGroup> canvasGroups = default;

        [UsedImplicitly] private EventHandlingControlMethod EventHandlingControlMethod
        {
            get => eventHandlingControlMethod;
            set => eventHandlingControlMethod = value;
        }
        [UsedImplicitly] private IEnumerable<BaseRaycaster> Raycasters
        {
            get => raycasters;
            set => raycasters = value.ToList();
        }
        [UsedImplicitly] private IEnumerable<KeyEventLayer> KeyEventLayers
        {
            get => keyEventLayers;
            set => keyEventLayers = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<Graphic> Graphics
        {
            get => graphics;
            set => graphics = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<Collider> Colliders
        {
            get => colliders;
            set => colliders = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<Collider2D> Collider2Ds
        {
            get => collider2Ds;
            set => collider2Ds = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<CanvasGroup> CanvasGroups
        {
            get => canvasGroups;
            set => canvasGroups = value.ToList();
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            HandleActivation();
            return ObservableFactory.ReturnMessage(this);
        }

        private void HandleActivation()
        {
            var activate = EventHandlingControlMethod == EventHandlingControlMethod.Activate;
            Raycasters.Where(x => x.enabled != activate).ToList().ForEach(x => x.enabled = activate);
            KeyEventLayers.Where(x => x.enabled != activate).ToList().ForEach(x => x.enabled = activate);
            Graphics.Where(x => x.raycastTarget != activate).ToList().ForEach(x => x.raycastTarget = activate);
            Colliders.Where(x => x.enabled != activate).ToList().ForEach(x => x.enabled = activate);
            Collider2Ds.Where(x => x.enabled != activate).ToList().ForEach(x => x.enabled = activate);
            CanvasGroups.Where(x => x.blocksRaycasts != activate).ToList().ForEach(x => x.blocksRaycasts = activate);
        }
    }

    [PublicAPI]
    public enum EventHandlingControlMethod
    {
        Activate,
        Deactivate,
    }
}
