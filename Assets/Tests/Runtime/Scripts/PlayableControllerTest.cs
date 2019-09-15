using System.Collections;
using NUnit.Framework;
using UniFlow.Connector.Controller;
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

        private void AssertPlayableController(Messages messages)
        {
            Assert.AreEqual(2, messages.Count);

            {
                Assert.True(messages[0].Is<PlayableController.Message>());
                var message = messages[0].As<PlayableController.Message>();
                Assert.IsInstanceOf<PlayableDirector>(message.Sender.PlayableDirector);
            }

            {
                Assert.True(messages[1].Is<TimelineSignal.Message>());
                var message = messages[1].As<TimelineSignal.Message>();
                Assert.IsInstanceOf<TimelineSignal>(message.Sender);
                var timelineEvent = message.Data;
                Assert.True(timelineEvent != default);
                Assert.AreEqual("PlayableControllerTest", timelineEvent.stringParameter);
            }

            HasAssert = true;
        }
    }
}