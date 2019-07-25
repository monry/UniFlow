using System.Collections;
using EventConnector.Connector;
using EventConnector.Message;
using NUnit.Framework;
using UnityEngine.Playables;
using UnityEngine.TestTools;

namespace EventConnector
{
    public class PlayableControllerTest : EventConnectorTestBase
    {

        [UnityTest]
        public IEnumerator PlayableController()
        {
            yield return
                RunAssert(
                    "PlayableController",
                    AssertPlayableController,
                    null,
                    1.1
                );
        }

        private void AssertPlayableController(EventMessages eventMessages)
        {
            Assert.AreEqual(2, eventMessages.Count);

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[0].sender);
            Assert.IsInstanceOf<PlayableControllerEventData>(eventMessages[0].eventData);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[1].sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[1].eventData);
            var timelineEvent = eventMessages[1].eventData as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("PlayableControllerTest", timelineEvent.StringParameter);
            HasAssert = true;
        }
    }
}