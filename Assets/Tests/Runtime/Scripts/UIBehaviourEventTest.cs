using System.Collections;
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

        private void AssertUIBehaviourEventTrigger(Messages messages)
        {
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<UIBehaviourEventTrigger.Message>());
            var message = messages[0].As<UIBehaviourEventTrigger.Message>();
            Assert.IsInstanceOf<Image>(message.Sender.UIBehaviour);
            Assert.IsInstanceOf<PointerEventData>(message.Data);
            HasAssert = true;
        }
    }
}