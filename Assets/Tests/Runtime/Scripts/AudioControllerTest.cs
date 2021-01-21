using System;
using System.Collections;
using System.Collections.Generic;
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

        private void AssertAudioEvent(IEnumerable<IConnector> sentConnectors, Action<AudioSource> callback)
        {
            var connectors = sentConnectors.ToList();

            Assert.NotNull(connectors);
            Assert.AreEqual(3, connectors.Count);

            var connector = connectors[1] as AudioController;
            Assert.NotNull(connector);

            callback(connector.AudioSource);

            HasAssert = true;
        }
    }
}
