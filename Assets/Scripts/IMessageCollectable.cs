using System.Collections.Generic;

namespace UniFlow
{
    public interface IMessageCollectable
    {
        // Editor 専用にする？
        IEnumerable<CollectableMessageAnnotation> GetMessageCollectableAnnotations();

        void Collect();
        void RegisterCollectDelegates();
    }
}
