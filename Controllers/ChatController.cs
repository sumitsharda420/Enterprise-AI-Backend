using DotNetAiChat.Models;
using DotNetAiChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAiChat.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IAiChatService _aiService;

        public ChatController(IAiChatService aiService)
        {
            _aiService = aiService;
        }

       
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Chat(ChatRequest request)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var result = await _aiService.GetResponseAsync(
                request.SessionId,
                request.Message,
                cts.Token);

            return Ok(new ChatResponse(result));
        }
    }
}
