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

// ReSharper disable Unity.LoadSceneUnknownSceneName

namespace EventConnector
{
    public class EventConnectorTest : ZenjectIntegrationTestFixture
    {
        private const string ScenePath = "Tests/PlayMode/Scenes/Basic/";
        private bool HasAssert { get; set; }

        [SetUp]
        public void SetUp()
        {
            HasAssert = false;
        }

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

        [UnityTest]
        public IEnumerator TimelineSignal()
        {
            yield return
                RunAssert(
                    "TimelineSignal",
                    AssertTimelineSignal,
                    null,
                    1.1
                );
        }

        [UnityTest]
        public IEnumerator PlayableController()
        {
            yield return
                RunAssert(
                    "PlayableController",
                    AssertPlayableController,
                    null,
                    1.1
                );
        }

        private void AssertAll(EventMessages eventMessages)
        {
            Assert.AreEqual(5, eventMessages.Count);

            Assert.IsInstanceOf<Image>(eventMessages[0].sender);
            Assert.IsInstanceOf<PointerEventData>(eventMessages[0].eventData);

            Assert.IsInstanceOf<Animator>(eventMessages[1].sender);
            Assert.IsInstanceOf<AnimatorTriggerEventData>(eventMessages[1].eventData);

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
            Assert.IsInstanceOf<PlayableControllerEventData>(eventMessages[3].eventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[4].sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[4].eventData);
            var timelineEvent = eventMessages[4].eventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual(999, timelineEvent.IntParameter);
            HasAssert = true;
        }

        private void AssertUIBehaviourEventTrigger(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<Image>(eventMessages[0].sender);
            Assert.IsInstanceOf<PointerEventData>(eventMessages[0].eventData);
            HasAssert = true;
        }

        private void AssertAnimationEvent(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<AnimationEvent>(eventMessages[0].sender);
            Assert.IsInstanceOf<UnityEngine.AnimationEvent>(eventMessages[0].eventData);
            var animationEvent = eventMessages[0].eventData as UnityEngine.AnimationEvent;
            Assert.NotNull(animationEvent);
            Assert.AreEqual(11.1f, animationEvent.floatParameter);
            Assert.AreEqual(22, animationEvent.intParameter);
            Assert.AreEqual("33", animationEvent.stringParameter);
            Assert.IsInstanceOf<Material>(animationEvent.objectReferenceParameter);
            Assert.AreEqual("AnimationEvent", animationEvent.objectReferenceParameter.name);
            HasAssert = true;
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

        private void AssertTimelineSignal(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[0].sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[0].eventData);
            var timelineEvent = eventMessages[0].eventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("TimelineSignalTest", timelineEvent.StringParameter);
            HasAssert = true;
        }

        private void AssertPlayableController(EventMessages eventMessages)
        {
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[0].sender);
            Assert.IsInstanceOf<PlayableControllerEventData>(eventMessages[0].eventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[1].sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[1].eventData);
            var timelineEvent = eventMessages[1].eventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("PlayableControllerTest", timelineEvent.StringParameter);
            HasAssert = true;
        }

        private IEnumerator RunAssert(string sceneName, Action<EventMessages> assertCallback, Action beforeAssertCallback, double waitBeforeAssert = 0, int invokeCount = 1)
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync($"{ScenePath}{sceneName}", LoadSceneMode.Additive);
            yield return Observable.TimerFrame(10);
            for (var i = 0; i < invokeCount; i++)
            {
                beforeAssertCallback?.Invoke();
                if (waitBeforeAssert > 0)
                {
                    yield return Observable.Timer(TimeSpan.FromSeconds(waitBeforeAssert));
                }
                assertCallback(Object.FindObjectOfType<TestReceiver>().SentEventMessages);
            }
            Assert.AreEqual(invokeCount, Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync($"{ScenePath}{sceneName}");
            PostInstall();
        }
    }
}