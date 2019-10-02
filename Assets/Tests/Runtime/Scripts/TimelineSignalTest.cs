using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Event;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class TimelineSignalTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator TimelineSignal()
        {
            yield return
                RunAssert(
                    "TimelineSignal",
                    AssertTimelineSignal,
                    null,
                    1.1
                );
        }

        private void AssertTimelineSignal(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(2, connectors.Count);

            var connector = connectors[0] as TimelineSignal;
            Assert.NotNull(connector);
            HasAssert = true;
        }
    }
}
