using System.Collections;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace EventConnector
{
    public class TimelineSignalTest : EventConnectorTestBase
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
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[0].EventData);
            var timelineEvent = eventMessages[0].EventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("TimelineSignalTest", timelineEvent.StringParameter);
            HasAssert = true;
        }
    }
}