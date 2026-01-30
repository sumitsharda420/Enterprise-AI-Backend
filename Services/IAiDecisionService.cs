using DotNetAiChat.Models;

namespace DotNetAiChat.Services
{
    public interface IAiDecisionService
    {
        Task<DecisionResponse> SuggestAsync(
            DecisionRequest request,
            CancellationToken ct);
    }
   
}
