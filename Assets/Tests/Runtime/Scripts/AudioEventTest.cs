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
    public class AudioEventTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator PlayEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Play),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "Play");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => audioSource.Play());
                    },
                    0.5
                );
        }

        [UnityTest]
        public IEnumerator StopEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Stop),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "Stop");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => audioSource.Play());
                        Observable.Timer(TimeSpan.FromSeconds(1.1)).Subscribe(_ => audioSource.Stop());
                    },
                    1.5
                );
        }

        [UnityTest]
        public IEnumerator PauseEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Pause),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "Pause");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => audioSource.Play());
                        Observable.Timer(TimeSpan.FromSeconds(1.1)).Subscribe(_ => audioSource.Pause());
                    },
                    1.5
                );
        }

        [UnityTest]
        public IEnumerator UnPauseEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.UnPause),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "UnPause");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.2)).Subscribe(_ => audioSource.Play());
                        Observable.Timer(TimeSpan.FromSeconds(1.2)).Subscribe(_ => audioSource.Pause());
                        Observable.Timer(TimeSpan.FromSeconds(1.7)).Subscribe(_ => audioSource.UnPause());
                    },
                    2.5
                );
        }

        [UnityTest]
        public IEnumerator PlayEvent_Automatically()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Play),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "PlayAutomatically");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.2)).Subscribe(_ => audioSource.enabled = true);
                    },
                    0.5
                );
        }

        [UnityTest]
        public IEnumerator StopEvent_Automatically()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Stop),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "StopAutomatically");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => audioSource.enabled = true);
                    },
                    8.5
                );
        }

        [UnityTest]
        public IEnumerator LoopEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    em => AssertAudioEvent(em, AudioEventType.Loop, 2),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "Loop");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.1)).Subscribe(_ => audioSource.Play());
                    },
                    17.0
                );
        }

        [UnityTest]
        public IEnumerator ComplexEvent()
        {
            yield return
                RunAssert(
                    "AudioEvent",
                    sentConnectors =>
                    {
                        var connectors = sentConnectors.ToList();
                        Assert.NotNull(sentConnectors);
                        Assert.AreEqual(5, connectors.Count);
                        Assert.AreEqual(AudioEventType.Play, (connectors[0] as AudioEvent)?.AudioEventType);
                        Assert.AreEqual(AudioEventType.Pause, (connectors[1] as AudioEvent)?.AudioEventType);
                        Assert.AreEqual(AudioEventType.UnPause, (connectors[2] as AudioEvent)?.AudioEventType);
                        Assert.AreEqual(AudioEventType.Stop, (connectors[3] as AudioEvent)?.AudioEventType);
                        HasAssert = true;
                    },
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioEvent")
                            .GetRootGameObjects()
                            .First(x => x.name == "Complex");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => audioSource.Play());
                        Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(_ => audioSource.Pause());
                        Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(_ => audioSource.UnPause());
                    },
                    15.0
                );
        }

        private void AssertAudioEvent(IEnumerable<IConnector> sentConnectors, AudioEventType audioEventType, int receiveCount = 1)
        {
            var connectors = sentConnectors.ToList();

            Assert.NotNull(connectors);
            Assert.AreEqual(2 * receiveCount, connectors.Count);
            Assert.AreEqual(receiveCount, Object.FindObjectOfType<TestReceiver>().ReceiveCount);

            var connector = connectors[0] as AudioEvent;
            Assert.NotNull(connector);
            Assert.IsInstanceOf<AudioSource>(connector.AudioSource);
            Assert.AreEqual(audioEventType, connector.AudioEventType);

            HasAssert = true;
        }
    }
}
