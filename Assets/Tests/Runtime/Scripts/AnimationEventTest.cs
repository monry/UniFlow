using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Event;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class AnimationEventTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator AnimationEvent()
        {
            yield return
                RunAssert(
                    "AnimationEvent",
                    AssertAnimationEvent,
                    null,
                    1.1
                );
        }

        private void AssertAnimationEvent(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(1, connectors.Count);

            var connector = connectors[0] as AnimationEvent;
            Assert.NotNull(connector);

            HasAssert = true;
        }
    }
}
