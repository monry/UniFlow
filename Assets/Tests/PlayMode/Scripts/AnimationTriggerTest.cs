using System.Collections;
using EventConnector.Message;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AnimationEvent = EventConnector.Connector.AnimationEvent;

namespace EventConnector
{
    public class AnimationTriggerTest : EventConnectorTestBase
    {
        [UnityTest]
        public IEnumerator AnimatorTrigger()
        {
            yield return
                RunAssert(
                    "AnimatorTrigger",
                    AssertAnimatorTrigger,
                    null,
                    1.1
                );
        }

        private void AssertAnimatorTrigger(EventMessages eventMessages)
        {
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<Animator>(eventMessages[0].sender);
            Assert.IsInstanceOf<AnimatorTriggerEventData>(eventMessages[0].eventData);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[1].sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[1].eventData);
            var animationEvent = eventMessages[1].eventData as UnityEngine.AnimationEvent;
            Assert.NotNull(animationEvent);
            Assert.AreEqual(22.2f, animationEvent.floatParameter);
            Assert.AreEqual(33, animationEvent.intParameter);
            Assert.AreEqual("44", animationEvent.stringParameter);
            Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
            Assert.AreEqual("AnimatorTrigger", animationEvent.objectReferenceParameter.name);
            HasAssert = true;
        }
    }
}