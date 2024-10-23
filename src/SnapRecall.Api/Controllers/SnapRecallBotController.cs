using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapRecall.Application.SnapRecallBotHandlers;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;

namespace SnapRecall.Api.Controllers
{
    [ApiController]
    [Route("bot")]
    public class SnapRecallBotController(ITelegramBot bot,IConfiguration configuration, ILogger<SnapRecallBotController> logger, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> PostAsync(
            // The secret token is optional, but it's highly recommended to use it.
            [FromHeader(Name = "X-Telegram-Bot-Api-Secret-Token")] string secretToken,
            [FromBody] Update update,
            CancellationToken cancellationToken)
        {
            if ("webhookToken" != secretToken)
            {
#if DEBUG
                logger.LogWarning("Failed access");
#endif
                this.Unauthorized();    
            }
            if (update == default)
            {
#if DEBUG
                logger.LogWarning("Invalid update detected");
#endif
                return this.BadRequest();
            } 
            
            await bot.OnUpdateAsync(update, cancellationToken);

            return await Task.FromResult(this.Ok());
        }
    }
}