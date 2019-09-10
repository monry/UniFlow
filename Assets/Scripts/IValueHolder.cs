namespace UniFlow
{
    public interface IValueHolder<out T>
    {
        T Value { get; }
    }
}