using DotNetAiChat.Models;
using DotNetAiChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAiChat.Controllers
{
    [ApiController]
    [Route("api/decision")]
    public class DecisionController : ControllerBase
    {
        private readonly IAiDecisionService _service;

        public DecisionController(IAiDecisionService service)
        {
            _service = service;
        }

        [HttpPost("suggest")]
        public async Task<IActionResult> Suggest(
            DecisionRequest request)
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(10));

            DecisionResponse result = await _service.SuggestAsync(request, cts.Token);
            return Ok(result);
        }
    }
}
