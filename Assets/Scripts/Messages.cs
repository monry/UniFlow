using System.Collections.Generic;
using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public class Messages : List<IMessage>
    {
        private Messages()
        {
        }

        public Messages Append(IMessage message)
        {
            Add(message);
            return this;
        }

        public Messages AppendRange(IEnumerable<IMessage> messages)
        {
            AddRange(messages);
            return this;
        }

        public static Messages Create()
        {
            return new Messages();
        }
    }
}