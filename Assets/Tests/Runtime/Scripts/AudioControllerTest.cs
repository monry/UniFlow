using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class AudioControllerTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator Play()
        {
            yield return
                RunAssert(
                    "AudioController",
                    em => AssertAudioEvent(
                        em,
                        AudioControlMethod.Play,
                        audioSource =>
                        {
                            Assert.True(audioSource.isPlaying);
                            Assert.Greater(audioSource.time, 0.0f);
                        }
                    ),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioController")
                            .GetRootGameObjects()
                            .First(x => x.name == "Play");
                        Assert.NotNull(go);
                        go.SetActive(true);
                    },
                    1.5
                );
        }

        [UnityTest]
        public IEnumerator Stop()
        {
            yield return
                RunAssert(
                    "AudioController",
                    em => AssertAudioEvent(
                        em,
                        AudioControlMethod.Stop,
                        audioSource =>
                        {
                            Assert.False(audioSource.isPlaying);
                            Assert.AreEqual(audioSource.time, 0.0f);
                        }
                    ),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioController")
                            .GetRootGameObjects()
                            .First(x => x.name == "Stop");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => audioSource.Play());
                    },
                    1.5
                );
        }

        [UnityTest]
        public IEnumerator Pause()
        {
            yield return
                RunAssert(
                    "AudioController",
                    em => AssertAudioEvent(
                        em,
                        AudioControlMethod.Pause,
                        audioSource =>
                        {
                            Assert.False(audioSource.isPlaying);
                            Assert.Greater(audioSource.time, 0.0f);
                        }
                    ),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioController")
                            .GetRootGameObjects()
                            .First(x => x.name == "Pause");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => audioSource.Play());
                    },
                    1.5
                );
        }

        [UnityTest]
        public IEnumerator UnPause()
        {
            yield return
                RunAssert(
                    "AudioController",
                    em => AssertAudioEvent(
                        em,
                        AudioControlMethod.UnPause,
                        audioSource =>
                        {
                            Assert.True(audioSource.isPlaying);
                            Assert.Greater(audioSource.time, 0.25f);
                        }
                    ),
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("AudioController")
                            .GetRootGameObjects()
                            .First(x => x.name == "UnPause");
                        Assert.NotNull(go);
                        go.SetActive(true);
                        var audioSource = go.GetComponent<AudioSource>();
                        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(_ => audioSource.Play());
                        Observable.Timer(TimeSpan.FromSeconds(0.75)).Subscribe(_ => audioSource.Pause());
                    },
                    1.5
                );
        }

        private void AssertAudioEvent(Messages messages, AudioControlMethod audioControlMethod, Action<AudioSource> callback)
        {
            Assert.NotNull(messages);
            Assert.AreEqual(2, messages.Count);

            Assert.True(messages[1].Is<AudioController.Message>());
            var message = messages[1].As<AudioController.Message>();

            Assert.NotNull(message.Data);

            Assert.AreEqual(audioControlMethod, message.Sender.AudioControlMethod);

            callback(message.Sender.AudioSource);

            HasAssert = true;
        }
    }
}