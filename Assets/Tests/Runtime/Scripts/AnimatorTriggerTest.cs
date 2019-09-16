using System.Collections;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UnityEngine;
using UnityEngine.TestTools;
using AnimationEvent = UniFlow.Connector.Event.AnimationEvent;

namespace UniFlow.Tests.Runtime
{
    public class AnimatorTriggerTest : UniFlowTestBase
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

        private void AssertAnimatorTrigger(Messages messages)
        {
            Assert.AreEqual(2, messages.Count);

            Assert.True(messages[0].Is<AnimatorTrigger.Message>());

            Assert.True(messages[1].Is<AnimationEvent.Message>());
            var message = messages[1].As<AnimationEvent.Message>();
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(message.Data);
            var animationEvent = message.Data;
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