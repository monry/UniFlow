using System.Collections;
using EventConnector;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.TestTools;
using UnityEngine.UI;
using AnimationEvent = EventConnector.Connector.AnimationEvent;

namespace EventConnector.Tests.Runtime
{
    public class AllTest : EventConnectorTestBase
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

        private void AssertAll(EventMessages eventMessages)
        {
            Assert.AreEqual(5, eventMessages.Count);

            Assert.IsInstanceOf<Image>(eventMessages[0].Sender);
            Assert.IsInstanceOf<PointerEventData>(eventMessages[0].EventData);

            Assert.IsInstanceOf<Animator>(eventMessages[1].Sender);
            Assert.IsInstanceOf<AnimatorTriggerEventData>(eventMessages[1].EventData);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[2].Sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[2].EventData);
            var animationEvent = eventMessages[2].EventData as UnityEngine.AnimationEvent;
            Assert.NotNull(animationEvent);
            Assert.AreEqual(222.2f, animationEvent.floatParameter);
            Assert.AreEqual(333, animationEvent.intParameter);
            Assert.AreEqual("444", animationEvent.stringParameter);
            Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
            Assert.AreEqual("All", animationEvent.objectReferenceParameter.name);

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[3].Sender);
            Assert.IsInstanceOf<PlayableControllerEventData>(eventMessages[3].EventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[4].Sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[4].EventData);
            var timelineEvent = eventMessages[4].EventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual(999, timelineEvent.IntParameter);
            HasAssert = true;
        }
    }
}