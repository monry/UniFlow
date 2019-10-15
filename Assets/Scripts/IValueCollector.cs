using System;

namespace UniFlow
{
    public interface IValueCollector
    {
        IConnector Connector { get; set; }
        string TypeString { get; set; }
        string ComposerKey { get; set; }
        string CollectorLabel { get; set; }
    }
}
