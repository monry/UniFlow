namespace UniFlow
{
    public interface IValueCollector
    {
        IConnector SourceConnector { get; set; }
        IConnector TargetConnector { get; set; }
        string TypeString { get; set; }
        string ComposerKey { get; set; }
        string CollectorKey { get; set; }
    }

    public interface IValueCollector<out TValue> : IValueCollector
    {
        TValue Collect();
        bool CanCollect();
    }
}
