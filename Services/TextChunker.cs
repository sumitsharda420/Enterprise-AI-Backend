namespace DotNetAiChat.Services
{
    public class TextChunker
    {
        private const int ChunkSize = 500;

        public List<string> Chunk(string text)
        {
            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += ChunkSize)
            {
                chunks.Add(
                    text.Substring(i, Math.Min(ChunkSize, text.Length - i))
                );
            }

            return chunks;
        }
    }
}
