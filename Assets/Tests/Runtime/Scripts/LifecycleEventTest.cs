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
using Object = UnityEngine.Object;

namespace UniFlow.Tests.Runtime
{
    public class LifecycleEventTest : UniFlowTestBase
    {
        private LifecycleEventType CurrentLifecycleEventType { get; set; }

        [UnityTest]
        public IEnumerator LifecycleStartEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.Start
            );
        }

        [UnityTest]
        public IEnumerator LifecycleUpdateEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.Update
            );
        }

        [UnityTest]
        public IEnumerator LifecycleFixedUpdateEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.FixedUpdate
            );
        }

        [UnityTest]
        public IEnumerator LifecycleLateUpdateEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.LateUpdate
            );
        }

        [UnityTest]
        public IEnumerator LifecycleDestroyEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.Destroy,
                go => Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => Object.Destroy(go))
            );
        }

        [UnityTest]
        public IEnumerator LifecycleEnableEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.Enable,
                go => Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => go.GetComponent<LifecycleEvent>().enabled = true)
            );
        }

        [UnityTest]
        public IEnumerator LifecycleDisableEvent()
        {
            yield return RunAssertInternal(
                LifecycleEventType.Disable,
                go => Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => go.SetActive(false))
            );
        }

        private IEnumerator RunAssertInternal(LifecycleEventType lifecycleEventType, Action<GameObject> callback = null)
        {
            yield return
                RunAssert(
                    "LifecycleEvent",
                    AssertLifecycleEvent,
                    () =>
                    {
                        CurrentLifecycleEventType = lifecycleEventType;
                        var go = SceneManager
                            .GetSceneByName("LifecycleEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == lifecycleEventType.ToString());
                        Assert.NotNull(go);
                        go.SetActive(true);
                        callback?.Invoke(go);
                    },
                    0.5
                );
        }

        private void AssertLifecycleEvent(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.NotNull(connectors);
            Assert.GreaterOrEqual(connectors.Count, 1);

            var connector = connectors[0] as LifecycleEvent;
            Assert.NotNull(connector);
            Assert.AreEqual(CurrentLifecycleEventType, connector.LifecycleEventType);

            HasAssert = true;
        }
    }
}
