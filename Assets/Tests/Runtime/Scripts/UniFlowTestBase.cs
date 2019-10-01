using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace UniFlow.Tests.Runtime
{
    public abstract class UniFlowTestBase : ZenjectIntegrationTestFixture
    {
        private const string ScenePath = "Tests/Runtime/Scenes/Basic/";
        protected bool HasAssert { private get; set; }

        [SetUp]
        public void SetUp()
        {
            HasAssert = false;
        }

        protected IEnumerator RunAssert(string sceneName, Action<IEnumerable<IConnector>> assertCallback, Action beforeAssertCallback, double waitBeforeAssert = 0, int invokeCount = 1)
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
                assertCallback(Object.FindObjectOfType<TestReceiver>().SentConnectors);
            }
            Assert.GreaterOrEqual(Object.FindObjectOfType<TestReceiver>().ReceiveCount, invokeCount);
            Assert.True(HasAssert);


            yield return SceneManager.UnloadSceneAsync($"{ScenePath}{sceneName}");
            PostInstall();
        }
    }
}
