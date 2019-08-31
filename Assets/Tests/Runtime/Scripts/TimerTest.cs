using System.Collections;
using UniFlow.Connector;
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

        private void AssertTimer(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.AreEqual(ConnectorType.Timer, eventMessages[0].ConnectorType);
            Assert.IsInstanceOf<Timer>(eventMessages[0].Sender);
            Assert.IsInstanceOf<float>(eventMessages[0].Data);
            Assert.AreEqual(1.0f, eventMessages[0].Data);

            HasAssert = true;
        }
    }
}