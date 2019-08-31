using System.Collections;
using UniFlow.Message;
using NUnit.Framework;
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

        private void AssertAnimatorTrigger(EventMessages eventMessages)
        {
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<Animator>(eventMessages[0].Sender);
            Assert.IsInstanceOf<AnimatorTriggerEventData>(eventMessages[0].Data);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[1].Sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[1].Data);
            var animationEvent = eventMessages[1].Data as UnityEngine.AnimationEvent;
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