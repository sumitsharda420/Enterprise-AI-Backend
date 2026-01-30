using Microsoft.AspNetCore.Mvc;

namespace DotNetAiChat.Controllers
{
    using DotNetAiChat.Models;
    using DotNetAiChat.Rag;
    using DotNetAiChat.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/rag")]
    public class RagController : ControllerBase
    {
        private readonly DocumentIngestionService _ingestion;
        private readonly TextChunker _chunker;
        private readonly EmbeddingService _embedding;
        private readonly InMemoryVectorStore _store;
        private readonly RagAnswerService _rag;

        public RagController(
            DocumentIngestionService ingestion,
            TextChunker chunker,
            EmbeddingService embedding,
            InMemoryVectorStore store,
            RagAnswerService rag)
        {
            _ingestion = ingestion;
            _chunker = chunker;
            _embedding = embedding;
            _store = store;
            _rag = rag;
        }

        // 1️⃣ Ingest document
        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody] string text)
        {
            var extracted = _ingestion.ExtractText(text);
            var chunks = _chunker.Chunk(extracted);

            foreach (var chunk in chunks)
            {
                var embedding = await _embedding.CreateEmbeddingAsync(chunk);

                _store.Add(new DocumentChunk(
                    Id: Guid.NewGuid().ToString(),
                    null,
                    Content: chunk,
                    Embedding: embedding
                ));
            }

            return Ok(new { Message = "Document ingested", Chunks = chunks.Count });
        }

        // 2️⃣ Ask question
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] RagQueryRequest request)
        {
            var result = await _rag.AskAsync(request.Question);
            return Ok(result);
        }
    }
}
