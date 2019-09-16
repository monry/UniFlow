using System.Collections;
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

        private void AssertInterval(Messages messages)
        {
            Assert.AreEqual(2, Object.FindObjectOfType<TestReceiver>().ReceiveCount);
            Assert.AreEqual(1, messages.Count);

            Assert.AreEqual(ConnectorType.Interval, messages[0].ConnectorType);
            var message = messages[0].As<Interval.Message>();
            Assert.IsInstanceOf<Interval>(message.Sender);
            Assert.IsInstanceOf<float>(message.Sender.Seconds);
            Assert.AreEqual(1.0f, message.Sender.Seconds);

            HasAssert = true;
        }
    }
}