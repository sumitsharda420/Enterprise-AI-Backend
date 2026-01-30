namespace DotNetAiChat.Memory
{
    using OpenAI.Chat;
    using System.Collections.Concurrent;

    public class InMemoryConversationStore : IConversationStore
    {
        private static readonly ConcurrentDictionary<string, List<ConversationItem>> _store = new();

        public List<ConversationItem> Get(string sessionId)
            => _store.GetOrAdd(sessionId, _ => new List<ConversationItem>());

        public void Save(string sessionId, List<ConversationItem> items)
            => _store[sessionId] = items;
    }
}
