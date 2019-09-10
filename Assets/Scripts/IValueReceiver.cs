namespace UniFlow
{
    public interface IValueReceiver<in T>
    {
        void Receive(T value);
    }
}