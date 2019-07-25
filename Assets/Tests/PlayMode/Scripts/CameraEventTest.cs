using System;
using System.Collections;
using System.Linq;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace EventConnector
{
    public class CameraEventTest : EventConnectorTestBase
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
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => go.transform.position = Vector3.zero);
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
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => go.transform.position = new Vector3(0.0f, 2.0f, 0.0f));
                    },
                    0.5
                );
        }

        private void AssertCameraBecomeVisibleEvent(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<CameraEvent>(eventMessages[0].sender);
            Assert.IsInstanceOf<CameraEventData>(eventMessages[0].eventData);

            HasAssert = true;
        }

        private void AssertCameraBecomeInvisibleEvent(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<CameraEvent>(eventMessages[0].sender);
            Assert.IsInstanceOf<CameraEventData>(eventMessages[0].eventData);

            HasAssert = true;
        }
    }
}