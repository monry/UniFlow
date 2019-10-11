namespace UniFlow
{
    public interface IInjectable<in T>
    {
        void Inject(T value);
    }
}
