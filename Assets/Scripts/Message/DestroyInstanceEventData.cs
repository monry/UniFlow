namespace UniFlow.Message
{
    public class DestroyInstanceEventData
    {
        private DestroyInstanceEventData()
        {
        }

        public static DestroyInstanceEventData Create()
        {
            return new DestroyInstanceEventData();
        }
    }
}