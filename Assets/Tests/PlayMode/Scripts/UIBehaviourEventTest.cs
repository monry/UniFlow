using System.Collections;
using NUnit.Framework;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EventConnector
{
    public class UIBehaviourEventTest : EventConnectorTestBase
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

        private void AssertUIBehaviourEventTrigger(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<Image>(eventMessages[0].sender);
            Assert.IsInstanceOf<PointerEventData>(eventMessages[0].eventData);
            HasAssert = true;
        }
    }
}