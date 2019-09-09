namespace UniFlow.Message
{
    public class ActivationControllerEventData
    {
        private ActivationControllerEventData(bool activated)
        {
            Activated = activated;
        }

        public bool Activated { get; }

        public static ActivationControllerEventData Create(bool activated)
        {
            return new ActivationControllerEventData(activated);
        }
    }
}