namespace UniFlow
{
    public interface IValuePublisher<out T>
    {
        T Publish();
    }
}