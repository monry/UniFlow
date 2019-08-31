using UniFlow.Connector;
using UniFlow.Connector.Controller;

namespace UniFlow.Message
{
    public struct AudioControllerEventData
    {
        private AudioControllerEventData(AudioControlMethod audioControlMethod)
        {
            AudioControlMethod = audioControlMethod;
        }

        public AudioControlMethod AudioControlMethod { get; }

        public static AudioControllerEventData Create(AudioControlMethod audioControlMethod) =>
            new AudioControllerEventData(audioControlMethod);
    }
}