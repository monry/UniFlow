using System.Collections;
using EventConnector.Connector;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace EventConnector.Tests.Runtime
{
    public class IntervalTest : EventConnectorTestBase
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

        private void AssertInterval(EventMessages eventMessages)
        {
            Assert.AreEqual(2, UnityEngine.Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.AreEqual(1, eventMessages.Count);

            Assert.AreEqual(EventType.Interval, eventMessages[0].EventType);
            Assert.IsInstanceOf<Interval>(eventMessages[0].Sender);
            Assert.IsInstanceOf<float>(eventMessages[0].EventData);
            Assert.AreEqual(1.0f, eventMessages[0].EventData);

            HasAssert = true;
        }
    }
}