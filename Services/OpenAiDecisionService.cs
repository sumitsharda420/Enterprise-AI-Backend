namespace DotNetAiChat.Services
{
    using DotNetAiChat.Models;
    using OpenAI.Chat;
    using System.Text.Json;

    public class OpenAiDecisionService : IAiDecisionService
    {
        private readonly ChatClient _client;

        public OpenAiDecisionService(IConfiguration config)
        {
            _client = new ChatClient(
                config["OpenAI:Model"],
                config["OPENAI_API_KEY"]);
        }

        public async Task<DecisionResponse> SuggestAsync(
            DecisionRequest request,
            CancellationToken ct)
        {
            var systemPrompt = """
You are an enterprise decision engine.

MANDATORY RULES:
- You MUST return a non-empty "suggestion".
- "suggestion" must be a concrete business action.
- You MUST compute confidence based on clarity of context.
- Confidence rules:
  - Clear context → confidence >= 0.7
  - Partial context → confidence between 0.4 and 0.69
- If confidence < 0.7 then needsHumanApproval = true.
- Empty strings are NOT allowed.
- Zero confidence is NOT allowed unless context is empty.
- NEVER return placeholder or empty values.
- Return ONLY valid JSON. No explanation.
""";

            var userPrompt =
     "Context:\n" +
     request.Context + "\n\n" +
     "Rules:\n- " + string.Join("\n- ", request.Rules) + "\n\n" +
     "Return JSON:\n" +
     "{ \"suggestion\": \"\", \"confidence\": 0.0, \"needsHumanApproval\": true }";

            var messages = new ChatMessage[]
            {
            ChatMessage.CreateSystemMessage(systemPrompt),
            ChatMessage.CreateUserMessage(userPrompt)
        };

            var response = await _client.CompleteChatAsync(messages, cancellationToken: ct);
            string r = response.Value.Content[0].Text;
            return JsonSerializer.Deserialize<DecisionResponse>(
                response.Value.Content[0].Text)!;
        }
    }
}
