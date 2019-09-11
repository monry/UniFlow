using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AnimationEvent = UniFlow.Connector.Event.AnimationEvent;

namespace UniFlow.Tests.Runtime
{
    public class AnimationEventTest : UniFlowTestBase
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

        private void AssertAnimationEvent(Messages messages)
        {
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<AnimationEvent.Message>());
            var message = messages[0].As<AnimationEvent.Message>();
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(message.Data);
            var animationEvent = message.Data;
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