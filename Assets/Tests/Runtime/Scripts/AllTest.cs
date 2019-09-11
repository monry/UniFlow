using System.Collections;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UniFlow.Connector.Event;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.TestTools;
using UnityEngine.UI;
using AnimationEvent = UniFlow.Connector.Event.AnimationEvent;

namespace UniFlow.Tests.Runtime
{
    public class AllTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator All()
        {
            yield return
                RunAssert(
                    "All",
                    AssertAll,
                    () =>
                        typeof(IPointerDownHandler)
                            .GetMethod("OnPointerDown")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerDownTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1
                );
        }

        [UnityTest]
        public IEnumerator Multiple()
        {
            yield return
                RunAssert(
                    "All",
                    AssertAll,
                    () =>
                        typeof(IPointerDownHandler)
                            .GetMethod("OnPointerDown")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerDownTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1,
                    2
                );
        }

        private void AssertAll(Messages messages)
        {
            Assert.AreEqual(5, messages.Count);

            {
                Assert.True(messages[0].Is<UIBehaviourEventTrigger.Message>());
                var message = messages[0].As<UIBehaviourEventTrigger.Message>();
                Assert.AreEqual(message.ConnectorType, ConnectorType.UIBehaviourEventTrigger);
                Assert.IsInstanceOf<Image>(message.Sender.UIBehaviour);
                Assert.IsInstanceOf<PointerEventData>(message.Data);
            }

            {
                Assert.True(messages[1].Is<AnimatorTrigger.Message>());
                var message = messages[1].As<AnimatorTrigger.Message>();
                Assert.IsInstanceOf<Animator>(message.Sender.Animator);
            }

            {
                Assert.True(messages[2].Is<AnimationEvent.Message>());
                var message = messages[2].As<AnimationEvent.Message>();
                Assert.IsInstanceOf<Animator>(message.Sender.Animator);
                Assert.IsInstanceOf<UnityEngine.AnimationEvent>(message.Data);
                var animationEvent = message.Data;
                Assert.NotNull(animationEvent);
                Assert.AreEqual(222.2f, animationEvent.floatParameter);
                Assert.AreEqual(333, animationEvent.intParameter);
                Assert.AreEqual("444", animationEvent.stringParameter);
                Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
                Assert.AreEqual("All", animationEvent.objectReferenceParameter.name);
            }

            {
                Assert.True(messages[3].Is<PlayableController.Message>());
                var message = messages[3].As<PlayableController.Message>();
                Assert.IsInstanceOf<PlayableDirector>(message.Sender.PlayableDirector);
            }

            {
                Assert.True(messages[4].Is<TimelineSignal.Message>());
                var message = messages[4].As<TimelineSignal.Message>();
                var timelineEvent = message.Data;
                Assert.AreEqual(999, timelineEvent.intParameter);
            }

            HasAssert = true;
        }
    }
}