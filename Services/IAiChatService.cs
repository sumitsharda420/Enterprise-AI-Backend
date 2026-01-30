namespace DotNetAiChat.Services
{
    public interface IAiChatService
    {
        Task<string> GetResponseAsync(string userInput);
        Task<string> GetResponseAsync(string userInput, CancellationToken ct);
        Task<string> GetResponseAsync(
    string sessionId,
    string userInput,
    CancellationToken ct);
    }
}
