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
    public class AudioControllerTest : EventConnectorTestBase
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

        private void AssertAudioEvent(EventMessages eventMessages, AudioControlMethod audioControlMethod, Action<AudioSource> callback)
        {
            Assert.NotNull(eventMessages);
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<AudioSource>(eventMessages[1].Sender);
            Assert.IsInstanceOf<AudioControllerEventData>(eventMessages[1].EventData);

            Assert.NotNull(eventMessages[1].EventData);

            Assert.AreEqual(audioControlMethod, ((AudioControllerEventData) eventMessages[1].EventData).AudioControlMethod);

            callback(eventMessages[1].Sender as AudioSource);

            HasAssert = true;
        }
    }
}