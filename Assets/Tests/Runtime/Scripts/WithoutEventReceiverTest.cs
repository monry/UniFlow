using System;
using System.Collections;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;
using AnimationEvent = EventConnector.Connector.AnimationEvent;
using Object = UnityEngine.Object;

namespace EventConnector.Tests.Runtime
{
    public class WithoutEventReceiverTest : ZenjectIntegrationTestFixture
    {
        private const string ScenePath = "Tests/Runtime/Scenes/WithoutReceiver/";
        private bool HasAssert { get; set; }

        [SetUp]
        public void SetUp()
        {
            HasAssert = false;
        }

        [UnityTest]
        public IEnumerator WithoutReceiver()
        {
            yield return
                RunAssert(
                    "WithoutReceiver",
                    AssertWithoutReceiver,
                    () =>
                        typeof(IPointerEnterHandler)
                            .GetMethod("OnPointerEnter")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerEnterTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1
                );
        }

        [UnityTest]
        public IEnumerator Multiple()
        {
            yield return
                RunAssert(
                    "WithoutReceiver",
                    AssertWithoutReceiver,
                    () =>
                        typeof(IPointerEnterHandler)
                            .GetMethod("OnPointerEnter")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerEnterTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1,
                    2
                );
        }

        private void AssertWithoutReceiver(EventMessages eventMessages)
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

        private IEnumerator RunAssert(string sceneName, Action<EventMessages> assertCallback, Action beforeAssertCallback, double waitBeforeAssert = 0, int invokeCount = 1)
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync($"{ScenePath}{sceneName}", LoadSceneMode.Additive);
            yield return Observable.TimerFrame(10).StartAsCoroutine();
            for (var i = 0; i < invokeCount; i++)
            {
                beforeAssertCallback?.Invoke();
                if (waitBeforeAssert > 0)
                {
                    yield return Observable.Timer(TimeSpan.FromSeconds(waitBeforeAssert)).StartAsCoroutine();
                }
                assertCallback(Object.FindObjectOfType<TestConnector>().LatestEventMessages);
            }
            Assert.AreEqual(invokeCount, Object.FindObjectOfType<TestConnector>().InvokedCount);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync($"{ScenePath}{sceneName}");
            PostInstall();
        }
    }
}