using System.Collections;
using UniFlow.Connector;
using NUnit.Framework;
using UniFlow.Connector.Logic;
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

        private void AssertInterval(EventMessages eventMessages)
        {
            Assert.AreEqual(2, UnityEngine.Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.AreEqual(1, eventMessages.Count);

            Assert.AreEqual(ConnectorType.Interval, eventMessages[0].ConnectorType);
            Assert.IsInstanceOf<Interval>(eventMessages[0].Sender);
            Assert.IsInstanceOf<float>(eventMessages[0].Data);
            Assert.AreEqual(1.0f, eventMessages[0].Data);

            HasAssert = true;
        }
    }
}