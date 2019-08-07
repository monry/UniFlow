using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AnimationEvent = EventConnector.Connector.AnimationEvent;

namespace EventConnector
{
    public class AnimationEventTest : EventConnectorTestBase
    {
        [UnityTest]
        public IEnumerator AnimationEvent()
        {
            yield return
                RunAssert(
                    "AnimationEvent",
                    AssertAnimationEvent,
                    null,
                    1.1
                );
        }

        private void AssertAnimationEvent(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[0].Sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[0].EventData);
            var animationEvent = eventMessages[0].EventData as UnityEngine.AnimationEvent;
            Assert.NotNull(animationEvent);
            Assert.AreEqual(11.1f, animationEvent.floatParameter);
            Assert.AreEqual(22, animationEvent.intParameter);
            Assert.AreEqual("33", animationEvent.stringParameter);
            Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
            Assert.AreEqual("AnimationEvent", animationEvent.objectReferenceParameter.name);
            HasAssert = true;
        }
    }
}