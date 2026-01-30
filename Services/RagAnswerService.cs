namespace DotNetAiChat.Services
{
    using DotNetAiChat.Models;
    using DotNetAiChat.Rag;
    using OpenAI.Chat;

    public class RagAnswerService
    {
        private readonly ChatClient _chat;
        private readonly EmbeddingService _embedding;
        private readonly InMemoryVectorStore _store;

        public RagAnswerService(
            IConfiguration config,
            EmbeddingService embedding,
            InMemoryVectorStore store)
        {
            _chat = new ChatClient("gpt-4o-mini", config["OPENAI_API_KEY"]);
            _embedding = embedding;
            _store = store;
        }
        public async Task<RagQueryResponse> AskAsync(
    string question,
    string documentId)
        {
            var queryEmbedding = await _embedding.CreateEmbeddingAsync(question);
            var topChunks = _store.Search(queryEmbedding, documentId);

            var context = string.Join("\n\n", topChunks.Select(c => c.Content));

            var messages = new ChatMessage []
            {
        ChatMessage.CreateSystemMessage(
            "Answer ONLY from the provided document context."
        ),
        ChatMessage.CreateUserMessage(
            $"Context:\n{context}\n\nQuestion:\n{question}"
        )
    };

            var response = await _chat.CompleteChatAsync(messages);

            return new RagQueryResponse(
                response.Value.Content[0].Text,
                topChunks.Select(c => c.Id).ToList()
            );
        }

        public async Task<RagQueryResponse> AskAsync(string question)
        {
            var queryEmbedding = await _embedding.CreateEmbeddingAsync(question);
            var topChunks = _store.Search(queryEmbedding);

            var context = string.Join("\n\n", topChunks.Select(c => c.Content));

            var messages = new ChatMessage[]
            {
            ChatMessage.CreateSystemMessage(
                "Answer ONLY using the provided context. If not found, say 'Not found in document'."
            ),
            ChatMessage.CreateUserMessage(
                $"Context:\n{context}\n\nQuestion:\n{question}"
            )
        };

            var response = await _chat.CompleteChatAsync(messages);

            return new RagQueryResponse(
                response.Value.Content[0].Text,
                topChunks.Select(c => c.Id).ToList()
            );
        }
    }
}
