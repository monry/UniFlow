namespace UniFlow
{
    public interface IValueHolder<out T> : IMessage
    {
        T Value { get; }
    }
}