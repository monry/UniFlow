using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UniFlow.Connector.Event;
using UnityEngine.Playables;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class PlayableControllerTest : UniFlowTestBase
    {

        [UnityTest]
        public IEnumerator PlayableController()
        {
            yield return
                RunAssert(
                    "PlayableController",
                    AssertPlayableController,
                    null,
                    1.1
                );
        }

        private void AssertPlayableController(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(3, connectors.Count);

            {
                var connector = connectors[0] as PlayableController;
                Assert.NotNull(connector);
                Assert.IsInstanceOf<PlayableDirector>(connector.PlayableDirector);
            }

            {
                var connector = connectors[1] as TimelineSignal;
                Assert.NotNull(connector);
            }

            HasAssert = true;
        }
    }
}
