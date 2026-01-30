namespace DotNetAiChat.Services
{
    using OpenAI.Embeddings;
    using System.Linq;
    using System.Collections.Generic;

    public class EmbeddingService
    {
        private readonly EmbeddingClient _client;

        public EmbeddingService(IConfiguration config)
        {
            _client = new EmbeddingClient(
                "text-embedding-3-small",
                config["OPENAI_API_KEY"]
            );
        }

        public async Task<float[]> CreateEmbeddingAsync(string text)
        {
            var response = await _client.GenerateEmbeddingAsync(text);
            var r = response.Value.ToFloats();
            ReadOnlyMemory<float> memory = response.Value.ToFloats();

            return memory.ToArray();
            // ✅ Correct conversion
            
        }
    }
}
