using System.Collections;
using UniFlow.Connector;
using UniFlow.Message;
using NUnit.Framework;
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

        private void AssertTimelineSignal(EventMessages eventMessages)
        {
            Assert.AreEqual(1, eventMessages.Count);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[0].Sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[0].Data);
            var timelineEvent = eventMessages[0].Data as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("TimelineSignalTest", timelineEvent.StringParameter);
            HasAssert = true;
        }
    }
}