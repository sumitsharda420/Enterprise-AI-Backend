using OpenAI.Chat;

namespace DotNetAiChat.Memory
{
    public interface IConversationStore
    {
        List<ConversationItem> Get(string sessionId);
        void Save(string sessionId, List<ConversationItem> items);
    }
}
