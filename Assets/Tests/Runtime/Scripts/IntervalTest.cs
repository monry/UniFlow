using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Logic;
using UnityEngine;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class IntervalTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator Interval()
        {
            yield return
                RunAssert(
                    "Interval",
                    AssertInterval,
                    null,
                    2.5
                );
        }

        private void AssertInterval(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(2, Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.AreEqual(4, connectors.Count);

            var connector = connectors[0] as Interval;
            Assert.NotNull(connector);
            Assert.IsInstanceOf<float>(connector.Seconds);
            Assert.AreEqual(1.0f, connector.Seconds);

            HasAssert = true;
        }
    }
}
