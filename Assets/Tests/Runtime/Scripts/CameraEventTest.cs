using System;
using System.Collections;
using System.Collections.Generic;
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

        private void AssertCameraBecomeVisibleEvent(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.NotNull(connectors);
            Assert.AreEqual(1, connectors.Count);

            var connector = connectors[0] as CameraEvent;
            Assert.NotNull(connector);

            Assert.AreEqual(CameraEventType.BecomeVisible, connector.CameraEventType);

            HasAssert = true;
        }

        private void AssertCameraBecomeInvisibleEvent(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.NotNull(connectors);
            Assert.AreEqual(1, connectors.Count);

            var connector = connectors[0] as CameraEvent;
            Assert.NotNull(connector);

            Assert.AreEqual(CameraEventType.BecomeInvisible, connector.CameraEventType);

            HasAssert = true;
        }
    }
}
