using System.Collections.Generic;

namespace UniFlow
{
    public interface IMessageComposable
    {
        IEnumerable<IComposableMessageAnnotation> GetMessageComposableAnnotations();
    }

    public static class MessageComposableExtensions
    {
        public static Message ComposeAll(this IMessageComposable messageComposable, Message message)
        {
            foreach (var messageComposableAnnotation in messageComposable.GetMessageComposableAnnotations())
            {
                message = messageComposableAnnotation.Compose(message);
            }

            return message;
        }
    }
}
