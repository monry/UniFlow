using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/ActivationController", (int) ConnectorType.LoadScene)]
    public class ActivationController : ConnectorBase
    {
        [SerializeField] private List<GameObject> targetGameObjects = default;
        [SerializeField] private List<MonoBehaviour> targetMonoBehaviours = default;
        [SerializeField] private bool controlSelfInstance = default;
        [SerializeField] private bool activated = default;

        [UsedImplicitly] public IEnumerable<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<MonoBehaviour> MonoBehaviours
        {
            get => targetMonoBehaviours;
            set => targetMonoBehaviours = value.ToList();
        }
        [ValueReceiver] public bool ControlSelfInstance
        {
            get => controlSelfInstance;
            set => controlSelfInstance = value;
        }
        [ValueReceiver] public bool Activated
        {
            get => activated;
            set => activated = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var gameObjects = GameObjects.Where(x => x.activeSelf != Activated).ToList();
            gameObjects.ForEach(x => x.SetActive(Activated));
            var monoBehaviours = MonoBehaviours.Where(x => x.enabled != Activated).ToList();
            monoBehaviours.ForEach(x => x.enabled = Activated);
            // ReSharper disable once InvertIf
            if (ControlSelfInstance)
            {
                if (gameObject.activeSelf != Activated)
                {
                    gameObject.SetActive(Activated);
                }
                var components = GetComponents<MonoBehaviour>().Where(x => x.enabled != Activated).ToList();
                components.ForEach(x => x.enabled = Activated);
            }

            return Observable.ReturnUnit();
        }
    }
}
