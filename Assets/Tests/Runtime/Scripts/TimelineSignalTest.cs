using System.Collections;
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

        private void AssertTimelineSignal(Messages messages)
        {
            Assert.AreEqual(1, messages.Count);

            Assert.True(messages[0].Is<TimelineSignal.Message>());
            var message = messages[0].As<TimelineSignal.Message>();
            Assert.IsInstanceOf<TimelineSignal>(message.Sender);
            var timelineEvent = message.Data;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("TimelineSignalTest", timelineEvent.stringParameter);
            HasAssert = true;
        }
    }
}