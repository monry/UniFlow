using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Logic;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class TimerTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator Timer()
        {
            yield return
                RunAssert(
                    "Timer",
                    AssertTimer,
                    null,
                    1.5
                );
        }

        private void AssertTimer(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(1, connectors.Count);

            var connector = connectors[0] as Timer;
            Assert.NotNull(connector);
            Assert.IsInstanceOf<float>(connector.Seconds);
            Assert.AreEqual(1.0f, connector.Seconds);

            HasAssert = true;
        }
    }
}
