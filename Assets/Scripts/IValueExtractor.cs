namespace UniFlow
{
    public interface IValueExtractor<in TValue>
    {
        void Extract(TValue value);
    }
}
