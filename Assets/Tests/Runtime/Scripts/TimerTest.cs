using System.Collections;
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

        private void AssertTimer(Messages messages)
        {
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<Timer.Message>());
            var message = messages[0].As<Timer.Message>();
            Assert.AreEqual(ConnectorType.Timer, message.ConnectorType);
            Assert.IsInstanceOf<Timer>(message.Sender);
            Assert.IsInstanceOf<float>(message.Sender.Seconds);
            Assert.AreEqual(1.0f, message.Sender.Seconds);

            HasAssert = true;
        }
    }
}