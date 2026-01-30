
namespace DotNetAiChat.Services
{
    using DotNetAiChat.Memory;
    using OpenAI.Chat;

    public class OpenAiChatService : IAiChatService
    {
        private readonly ChatClient _client;
        private readonly string _systemPrompt;
        private readonly IConversationStore _store;
        private readonly ContextWindowManager _context;
        private readonly ILogger _logger;
        public OpenAiChatService(IConfiguration config,
            ILogger<OpenAiChatService> logger, IConversationStore store,
    ContextWindowManager context)
        {
            var apiKey = config["OPENAI_API_KEY"];
            var model = config["OpenAI:Model"];
            _systemPrompt = config["OpenAI:SystemPrompt"];
            _store = store;
            _context = context;
            _logger = logger;
            _client = new ChatClient(model, apiKey);
        }
        private static ChatMessage ToChatMessage(ConversationItem item)
        {
            return item.Role switch
            {
                ConversationRole.System =>
                    ChatMessage.CreateSystemMessage(item.Content),

                ConversationRole.User =>
                    ChatMessage.CreateUserMessage(item.Content),

                ConversationRole.Assistant =>
                    ChatMessage.CreateAssistantMessage(item.Content),

                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public async Task<string> GetResponseAsync(
    string sessionId,
    string userInput,
    CancellationToken ct)
        {
            var history = _store.Get(sessionId);

            if (!history.Any())
            {
                history.Add(new ConversationItem(
                    ConversationRole.System,
                    _systemPrompt));
            }

            history.Add(new ConversationItem(
                ConversationRole.User,
                userInput));

            var trimmed = _context.Trim(history);

            var chatMessages = trimmed
                .Select(ToChatMessage)
                .ToList();

            var response = await _client.CompleteChatAsync(chatMessages, cancellationToken: ct);

            history.Add(new ConversationItem(
                ConversationRole.Assistant,
                response.Value.Content[0].Text));

            _store.Save(sessionId, history);

            return response.Value.Content[0].Text;
        }
        public async Task<string> GetResponseAsync(string userInput, CancellationToken ct)
        {
            var messages = new ChatMessage[]
            {
        ChatMessage.CreateSystemMessage(_systemPrompt),
        ChatMessage.CreateUserMessage(userInput)
    };

            try
            {
                var response = await _client.CompleteChatAsync(messages, cancellationToken: ct);
                return response.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AI service failed", ex);
            }
        }
        public async Task<string> GetResponseAsync(string userInput)
        {
            var messages = new ChatMessage[]
            {
            ChatMessage.CreateSystemMessage(_systemPrompt),
            ChatMessage.CreateUserMessage(userInput)
        };

            var response = await _client.CompleteChatAsync(messages);
            return response.Value.Content[0].Text;
        }
    }
}
