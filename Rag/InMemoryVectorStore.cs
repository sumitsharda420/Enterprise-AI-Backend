using DotNetAiChat.Models;

namespace DotNetAiChat.Rag
{
    public class InMemoryVectorStore
    {
        private readonly List<DocumentChunk> _chunks = new();

        public void Add(DocumentChunk chunk) => _chunks.Add(chunk);

        public List<DocumentChunk> Search(
            float[] queryEmbedding,
            string documentId,
            int topK = 3)
        {
            return _chunks
                .Where(c => c.DocumentId == documentId)
                .OrderByDescending(c => CosineSimilarity(c.Embedding, queryEmbedding))
                .Take(topK)
                .ToList();
        }

        public List<DocumentChunk> Search(float[] queryEmbedding, int topK = 3)
        {
            return _chunks
                .OrderByDescending(c => CosineSimilarity(c.Embedding, queryEmbedding))
                .Take(topK)
                .ToList();
        }

        private static float CosineSimilarity(float[] a, float[] b)
        {
            float dot = 0, magA = 0, magB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }

            return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
        }
    }
}
