using DotNetAiChat.Models;
using DotNetAiChat.Rag;
using DotNetAiChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAiChat.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentRagController : ControllerBase
    {
        private readonly DocumentTextExtractor _extractor;
        private readonly TextChunker _chunker;
        private readonly EmbeddingService _embedding;
        private readonly InMemoryVectorStore _store;
        private readonly RagAnswerService _rag;

        public DocumentRagController(
            DocumentTextExtractor extractor,
            TextChunker chunker,
            EmbeddingService embedding,
            InMemoryVectorStore store,
            RagAnswerService rag)
        {
            _extractor = extractor;
            _chunker = chunker;
            _embedding = embedding;
            _store = store;
            _rag = rag;
        }

        // 1️⃣ Upload document
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var documentId = Guid.NewGuid().ToString();
            var text = _extractor.ExtractText(file);
            var chunks = _chunker.Chunk(text);

            foreach (var chunk in chunks)
            {
                var embedding = await _embedding.CreateEmbeddingAsync(chunk);

                _store.Add(new DocumentChunk(
                    Guid.NewGuid().ToString(),
                    documentId,
                    chunk,
                    embedding
                ));
            }

            return Ok(new DocumentUploadResponse(documentId, chunks.Count));
        }

        // 2️⃣ Ask question from document
        [HttpPost("{documentId}/ask")]
        public async Task<IActionResult> Ask(
            string documentId,
            RagQueryRequest request)
        {
            var answer = await _rag.AskAsync(
                request.Question,
                documentId);

            return Ok(answer);
        }
    }
}
