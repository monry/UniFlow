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

// ReSharper disable Unity.LoadSceneUnknownSceneName

namespace EventConnector
{
    public class EventConnectorTest : ZenjectIntegrationTestFixture
    {
        private bool HasAssert { get; set; }

        [SetUp]
        public void SetUp()
        {
            HasAssert = false;
        }

        [UnityTest]
        public IEnumerator All()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("All", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            typeof(IPointerDownHandler)
                .GetMethod("OnPointerDown")?
                .Invoke(Object.FindObjectOfType<ObservablePointerDownTrigger>(), new object[] {new PointerEventData(EventSystem.current)});
            yield return UniTask.Delay(TimeSpan.FromSeconds(2.1)).ToCoroutine();
            AssertAll(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("All");
            PostInstall();
        }

        [UnityTest]
        public IEnumerator UIBehaviourEventTrigger()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("UIBehaviourEventTrigger", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            typeof(IPointerClickHandler)
                .GetMethod("OnPointerClick")?
                .Invoke(Object.FindObjectOfType<ObservablePointerClickTrigger>(), new object[] {new PointerEventData(EventSystem.current)});
            AssertUIBehaviourEventTrigger(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("UIBehaviourEventTrigger");
            PostInstall();
        }

        [UnityTest]
        public IEnumerator AnimationEvent()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("AnimationEvent", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            yield return UniTask.Delay(TimeSpan.FromSeconds(1.1)).ToCoroutine();
            AssertAnimationEvent(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("AnimationEvent");
            PostInstall();
        }

        [UnityTest]
        public IEnumerator AnimatorTrigger()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("AnimatorTrigger", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            yield return UniTask.Delay(TimeSpan.FromSeconds(1.1)).ToCoroutine();
            AssertAnimatorTrigger(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("AnimatorTrigger");
            PostInstall();
        }

        [UnityTest]
        public IEnumerator TimelineSignal()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("TimelineSignal", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            yield return UniTask.Delay(TimeSpan.FromSeconds(1.1)).ToCoroutine();
            AssertTimelineSignal(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("TimelineSignal");
            PostInstall();
        }

        [UnityTest]
        public IEnumerator PlayableController()
        {
            PreInstall();
            yield return SceneManager.LoadSceneAsync("PlayableController", LoadSceneMode.Additive);
            yield return UniTask.DelayFrame(1).ToCoroutine();
            yield return UniTask.Delay(TimeSpan.FromSeconds(1.1)).ToCoroutine();
            AssertPlayableController(Object.FindObjectOfType<TestComponent>().SentEventMessages);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync("PlayableController");
            PostInstall();
        }

        private void AssertAll(EventMessages eventMessages)
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
            Assert.IsInstanceOf<AnimatorTriggerEvent>(eventMessages[0].eventData);

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
            Assert.IsInstanceOf<TimelineEvent>(eventMessages[0].eventData);
            var timelineEvent = eventMessages[0].eventData as TimelineEvent;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("TimelineSignalTest", timelineEvent.StringParameter);
            HasAssert = true;
        }

        private void AssertPlayableController(EventMessages eventMessages)
        {
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[0].sender);
            Assert.IsInstanceOf<PlayableControllerEvent>(eventMessages[0].eventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[1].sender);
            Assert.IsInstanceOf<TimelineEvent>(eventMessages[1].eventData);
            var timelineEvent = eventMessages[1].eventData as TimelineEvent;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("PlayableControllerTest", timelineEvent.StringParameter);
            HasAssert = true;
        }
    }
}