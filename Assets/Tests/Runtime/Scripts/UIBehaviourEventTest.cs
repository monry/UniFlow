using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Event;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace UniFlow.Tests.Runtime
{
    public class UIBehaviourEventTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator UIBehaviourEventTrigger()
        {
            yield return
                RunAssert(
                    "UIBehaviourEventTrigger",
                    AssertUIBehaviourEventTrigger,
                    () =>
                        typeof(IPointerClickHandler)
                            .GetMethod("OnPointerClick")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerClickTrigger>(), new object[] {new PointerEventData(EventSystem.current)})
                );
        }

        private void AssertUIBehaviourEventTrigger(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(1, connectors.Count);

            var connector = connectors[0] as UIBehaviourEventTrigger;
            Assert.NotNull(connector);
            Assert.IsInstanceOf<Image>(connector.UIBehaviour);
            HasAssert = true;
        }
    }
}
