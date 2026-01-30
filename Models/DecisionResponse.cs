namespace DotNetAiChat.Models
{
    public record DecisionResponse(
        string suggestion,
        double confidence,
        bool needsHumanApproval
    );
}
