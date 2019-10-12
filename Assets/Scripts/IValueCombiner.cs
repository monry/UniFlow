namespace UniFlow
{
    public interface IValueCombiner<out TValue>
    {
        TValue Combine();
    }
}
