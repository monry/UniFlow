using System.Collections.Generic;

namespace UniFlow
{
    public interface IMessageComposable
    {
        // Editor 専用にする？
        IEnumerable<ComposableMessageAnnotation> GetMessageComposableAnnotations();
        Message Compose(Message message);
    }
}
