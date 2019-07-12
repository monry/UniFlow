using System;
using System.Collections;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UniRx.Async;
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

namespace EventConnector
{
    public class WithoutEventReceiverTest : ZenjectIntegrationTestFixture
    {
        private const string ScenePath = "Tests/PlayMode/Scenes/WithoutReceiver/";
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

            Assert.IsInstanceOf<Image>(eventMessages[0].sender);
            Assert.IsInstanceOf<PointerEventData>(eventMessages[0].eventData);

            Assert.IsInstanceOf<Animator>(eventMessages[1].sender);
            Assert.IsInstanceOf<AnimatorTriggerEvent>(eventMessages[1].eventData);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[2].sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[2].eventData);
            var animationEvent = eventMessages[2].eventData as UnityEngine.AnimationEvent;
            Assert.NotNull(animationEvent);
            Assert.AreEqual(222.2f, animationEvent.floatParameter);
            Assert.AreEqual(333, animationEvent.intParameter);
            Assert.AreEqual("444", animationEvent.stringParameter);
            Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
            Assert.AreEqual("All", animationEvent.objectReferenceParameter.name);

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[3].sender);
            Assert.IsInstanceOf<PlayableControllerEvent>(eventMessages[3].eventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[4].sender);
            Assert.IsInstanceOf<TimelineEvent>(eventMessages[4].eventData);
            var timelineEvent = eventMessages[4].eventData as TimelineEvent;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual(999, timelineEvent.IntParameter);
            HasAssert = true;
        }

        private IEnumerator RunAssert(string sceneName, Action<EventMessages> assertCallback, Action beforeAssertCallback, double waitBeforeAssert = 0, int invokeCount = 1)
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync($"{ScenePath}{sceneName}", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(10).ToCoroutine();
            for (var i = 0; i < invokeCount; i++)
            {
                beforeAssertCallback?.Invoke();
                if (waitBeforeAssert > 0)
                {
                    yield return UniTask.Delay(TimeSpan.FromSeconds(waitBeforeAssert)).ToCoroutine();
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