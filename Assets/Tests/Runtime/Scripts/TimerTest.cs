using System.Collections;
using UniFlow.Connector;
using NUnit.Framework;
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