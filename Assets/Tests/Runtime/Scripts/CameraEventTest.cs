using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Event;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class CameraEventTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator CameraBecomeVisibleEvent()
        {
            yield return
                RunAssert(
                    "CameraEvent",
                    AssertCameraBecomeVisibleEvent,
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("CameraEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "CubeWillVisible");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => go.GetComponent<MeshRenderer>().enabled = true);
                    },
                    0.5
                );
        }

        [UnityTest]
        public IEnumerator CameraBecomeInvisibleEvent()
        {
            yield return
                RunAssert(
                    "CameraEvent",
                    AssertCameraBecomeInvisibleEvent,
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("CameraEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "CubeWillInvisible");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => go.GetComponent<MeshRenderer>().enabled = false);
                    },
                    0.5
                );
        }

        private void AssertCameraBecomeVisibleEvent(Messages messages)
        {
            Assert.NotNull(messages);
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<CameraEvent.Message>());
            var message = messages[0].As<CameraEvent.Message>();
            Assert.IsInstanceOf<CameraEvent>(message.Sender);

            Assert.AreEqual(CameraEventType.BecomeVisible, message.Sender.CameraEventType);

            HasAssert = true;
        }

        private void AssertCameraBecomeInvisibleEvent(Messages messages)
        {
            Assert.NotNull(messages);
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<CameraEvent.Message>());
            var message = messages[0].As<CameraEvent.Message>();
            Assert.IsInstanceOf<CameraEvent>(message.Sender);

            Assert.AreEqual(CameraEventType.BecomeInvisible, message.Sender.CameraEventType);

            HasAssert = true;
        }
    }
}