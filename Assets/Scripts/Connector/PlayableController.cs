using EventConnector.Message;
using UnityEngine;
using UnityEngine.Playables;

namespace EventConnector.Connector
{
    [AddComponentMenu("Event Connector/PlayableController")]
    public class PlayableController : EventConnector
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<PlayableDirector>()")]
        private PlayableDirector playableDirector = default;
        private PlayableDirector PlayableDirector => playableDirector ? playableDirector : playableDirector = GetComponent<PlayableDirector>();

        protected override void Connect(EventMessages eventMessages)
        {
            PlayableDirector.Play();
            OnConnect(eventMessages.Append(EventMessage.Create(EventType.PlayableController, PlayableDirector, PlayableControllerEventData.Create())));
        }
    }
}