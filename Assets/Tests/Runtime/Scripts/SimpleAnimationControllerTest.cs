using System.Collections;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class SimpleAnimationControllerTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator Play()
        {
            yield return
                RunAssert(
                    "SimpleAnimationController",
                    sentConnectors =>
                    {
                        var connectors = sentConnectors.ToList();
                        Assert.That(connectors.Count, Is.EqualTo(2));
                        var connector = connectors[0] as SimpleAnimationController;
                        Assert.That(connector, Is.Not.Null);
                        Assert.That(connector.SimpleAnimation.isPlaying, Is.True);
                        HasAssert = true;
                    },
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("SimpleAnimationController")
                            .GetRootGameObjects()
                            .First(x => x.name == "Play");
                        Assert.That(go, Is.Not.Null);
                        go.SetActive(true);
                    },
                    0.5
                );
        }

        [UnityTest]
        public IEnumerator Stop()
        {
            yield return
                RunAssert(
                    "SimpleAnimationController",
                    sentConnectors =>
                    {
                        var connectors = sentConnectors.ToList();
                        Assert.That(connectors.Count, Is.EqualTo(4));
                        var connector = connectors[0] as SimpleAnimationController;
                        Assert.That(connector, Is.Not.Null);
                        Assert.That(connector.SimpleAnimation.isPlaying, Is.False);
                        HasAssert = true;
                    },
                    () =>
                    {
                        var go = SceneManager
                            .GetSceneByName("SimpleAnimationController")
                            .GetRootGameObjects()
                            .First(x => x.name == "Stop");
                        Assert.That(go, Is.Not.Null);
                        go.SetActive(true);
                    },
                    1.5
                );
        }
    }
}
