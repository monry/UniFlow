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

        private void AssertCameraBecomeVisibleEvent(EventMessages eventMessages)
        {
            Assert.NotNull(eventMessages);
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<CameraEvent>(eventMessages[0].sender);
            Assert.IsInstanceOf<CameraEventData>(eventMessages[0].eventData);

            Assert.NotNull(eventMessages[0].eventData);

            Assert.AreEqual(CameraEventType.BecomeVisible, ((CameraEventData) eventMessages[0].eventData).EventType);

            HasAssert = true;
        }

        private void AssertCameraBecomeInvisibleEvent(EventMessages eventMessages)
        {
            Assert.NotNull(eventMessages);
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<CameraEvent>(eventMessages[0].sender);
            Assert.IsInstanceOf<CameraEventData>(eventMessages[0].eventData);

            Assert.NotNull(eventMessages[0].eventData);

            Assert.AreEqual(CameraEventType.BecomeInvisible, ((CameraEventData) eventMessages[0].eventData).EventType);

            HasAssert = true;
        }
    }
}