using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/DestroyInstance", (int) ConnectorType.LoadScene)]
    public class DestroyInstance : ConnectorBase
    {
        [SerializeField] private List<Component> targetComponents = default;
        [SerializeField] private List<GameObject> targetGameObjects = default;

        [UsedImplicitly] public IEnumerable<Component> Components
        {
            get => targetComponents;
            set => targetComponents = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            var targets = Components.Concat<Object>(GameObjects).ToList();
            targets.ToList().ForEach(Destroy);
            return ObservableFactory.ReturnMessage(this);
        }
    }
}
