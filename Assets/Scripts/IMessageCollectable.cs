using System.Collections.Generic;

namespace UniFlow
{
    public interface IMessageCollectable
    {
        IEnumerable<ICollectableMessageAnnotation> GetMessageCollectableAnnotations();
    }
}
