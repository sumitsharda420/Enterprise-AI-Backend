namespace DotNetAiChat.Models
{
    public record RagQueryResponse(
    string Answer,
    List<string> Sources
);
}
