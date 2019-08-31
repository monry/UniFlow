using UniFlow.Connector;
using UniFlow.Connector.Controller;

namespace UniFlow.Message
{
    public class PlayableControllerEventData
    {
        private PlayableControllerEventData(PlayableControlMethod playableControlMethod)
        {
            PlayableControlMethod = playableControlMethod;
        }

        public PlayableControlMethod PlayableControlMethod { get; }

        public static PlayableControllerEventData Create(PlayableControlMethod playableControlMethod) =>
            new PlayableControllerEventData(playableControlMethod);
    }
}