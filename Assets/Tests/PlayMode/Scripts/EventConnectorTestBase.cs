using System;
using System.Collections;
using NUnit.Framework;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace EventConnector
{
    public abstract class EventConnectorTestBase : ZenjectIntegrationTestFixture
    {
        private const string ScenePath = "Tests/PlayMode/Scenes/Basic/";
        protected bool HasAssert { private get; set; }

        [SetUp]
        public void SetUp()
        {
            HasAssert = false;
        }

        protected IEnumerator RunAssert(string sceneName, Action<EventMessages> assertCallback, Action beforeAssertCallback, double waitBeforeAssert = 0, int invokeCount = 1)
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
                assertCallback(Object.FindObjectOfType<TestReceiver>().SentEventMessages);
            }
            Assert.AreEqual(invokeCount, Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.True(HasAssert);
            yield return SceneManager.UnloadSceneAsync($"{ScenePath}{sceneName}");
            PostInstall();
        }
    }
}