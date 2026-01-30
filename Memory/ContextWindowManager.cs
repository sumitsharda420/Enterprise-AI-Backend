namespace DotNetAiChat.Memory
{
    using OpenAI.Chat;

    public class ContextWindowManager
    {
        private const int MaxMessages = 10;

        public List<ConversationItem> Trim(List<ConversationItem> items)
        {
            var system = items.FirstOrDefault(x => x.Role == ConversationRole.System);

            var recent = items
                .Where(x => x.Role != ConversationRole.System)
                .TakeLast(MaxMessages - 1)
                .ToList();

            return system != null
                ? new List<ConversationItem> { system }.Concat(recent).ToList()
                : recent;
        }
    }
}
