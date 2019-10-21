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
    public class DestroyInstance : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private List<Object> objects = new List<Object>();

        [UsedImplicitly] public IList<Object> Objects
        {
            get => objects;
            set => objects = value.ToList();
        }

        [SerializeField] private ObjectCollector objectCollector = new ObjectCollector();
        private ObjectCollector ObjectCollector => objectCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            Objects.ToList().ForEach(Destroy);
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(ObjectCollector, Objects.Add, "Object"),
            };
    }
}
