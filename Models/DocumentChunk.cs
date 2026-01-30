namespace DotNetAiChat.Models
{
    public record DocumentChunk(
     string Id,
     string DocumentId,
     string Content,
     float[] Embedding
 );
}
