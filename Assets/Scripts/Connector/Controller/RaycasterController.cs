using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/RaycasterController", (int) ConnectorType.RaycasterController)]
    public class RaycasterController : ConnectorBase
    {
        [SerializeField] private RaycasterControlMethod raycasterControlMethod = RaycasterControlMethod.Activate;
        [SerializeField] private List<BaseRaycaster> raycasters = default;

        [UsedImplicitly] private RaycasterControlMethod RaycasterControlMethod
        {
            get => raycasterControlMethod;
            set => raycasterControlMethod = value;
        }
        [UsedImplicitly] private IEnumerable<BaseRaycaster> Raycasters
        {
            get => raycasters;
            set => raycasters = value.ToList();
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            HandleActivation();
            return Observable.ReturnUnit();
        }

        private void HandleActivation()
        {
            var targets = Raycasters.Where(x => x.enabled != (RaycasterControlMethod == RaycasterControlMethod.Activate)).ToList();
            targets.ForEach(x => x.enabled = RaycasterControlMethod == RaycasterControlMethod.Activate);
        }
    }

    [PublicAPI]
    public enum RaycasterControlMethod
    {
        Activate,
        Deactivate,
    }
}
