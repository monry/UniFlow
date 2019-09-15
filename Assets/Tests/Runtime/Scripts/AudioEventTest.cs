using System;
using System.Collections;
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
                    em =>
                    {
                        Assert.NotNull(em);
                        Assert.AreEqual(4, em.Count);
                        Assert.AreEqual(AudioEventType.Play, em[0].As<AudioEvent.Message>().Sender.AudioEventType);
                        Assert.AreEqual(AudioEventType.Pause, em[1].As<AudioEvent.Message>().Sender.AudioEventType);
                        Assert.AreEqual(AudioEventType.UnPause, em[2].As<AudioEvent.Message>().Sender.AudioEventType);
                        Assert.AreEqual(AudioEventType.Stop, em[3].As<AudioEvent.Message>().Sender.AudioEventType);
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

        private void AssertAudioEvent(Messages messages, AudioEventType audioEventType, int receiveCount = 1)
        {
            Assert.NotNull(messages);
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(receiveCount, Object.FindObjectOfType<TestReceiver>().ReceiveCount);

            Assert.True(messages[0].Is<AudioEvent.Message>());
            var message = messages[0].As<AudioEvent.Message>();
            Assert.IsInstanceOf<AudioSource>(message.Sender.AudioSource);

            Assert.NotNull(message.Data);

            Assert.AreEqual(audioEventType, message.Sender.AudioEventType);

            HasAssert = true;
        }
    }
}