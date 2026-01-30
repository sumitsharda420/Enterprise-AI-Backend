namespace DotNetAiChat.Models
{
    public record DecisionRequest(
    string Context,
    List<string> Rules
);
}
