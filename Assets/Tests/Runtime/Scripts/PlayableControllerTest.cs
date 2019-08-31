using System.Collections;
using UniFlow.Connector;
using UniFlow.Message;
using NUnit.Framework;
using UniFlow.Connector.Event;
using UnityEngine.Playables;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class PlayableControllerTest : UniFlowTestBase
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

            Assert.IsInstanceOf<PlayableDirector>(eventMessages[0].Sender);
            Assert.IsInstanceOf<PlayableControllerEventData>(eventMessages[0].Data);

            Assert.IsInstanceOf<TimelineSignal>(eventMessages[1].Sender);
            Assert.IsInstanceOf<TimelineEventData>(eventMessages[1].Data);
            var timelineEvent = eventMessages[1].Data as TimelineEventData;
            Assert.NotNull(timelineEvent);
            Assert.AreEqual("PlayableControllerTest", timelineEvent.StringParameter);
            HasAssert = true;
        }
    }
}