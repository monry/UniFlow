using System.Collections;
using EventConnector.Connector;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace EventConnector.Tests.Runtime
{
    public class TimerTest : EventConnectorTestBase
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

        private void AssertTimer(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.AreEqual(EventType.Timer, eventMessages[0].EventType);
            Assert.IsInstanceOf<Timer>(eventMessages[0].Sender);
            Assert.IsInstanceOf<float>(eventMessages[0].EventData);
            Assert.AreEqual(1.0f, eventMessages[0].EventData);

            HasAssert = true;
        }
    }
}