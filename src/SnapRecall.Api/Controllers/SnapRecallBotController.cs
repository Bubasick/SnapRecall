using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

namespace SnapRecall.Api.Controllers
{
    [ApiController]
    [Route("bot")]
    public class SnapRecallBotController(ITelegramBotClient client, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(
            // The secret token is optional, but it's highly recommended to use it.
            [FromHeader(Name = "X-Telegram-Bot-Api-Secret-Token")] string secretToken,
            [FromBody] Update update)
        {

            long chatId = update.Message.Chat.Id; // Target chat Id
            client.SendMessage(chatId, "Hello World!"); // Send a message
            if (update is null)
            {
                return BadRequest();
            }
            // Check if the secret token is valid
            // Process your update
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Start()
        //{
        //    var updates = client.GetUpdates();
        //    while (true)
        //    {
        //        if (updates.Any())
        //        {
        //            foreach (var update in updates)
        //            {
        //                long chatId = update.Message.Chat.Id; // Target chat Id
        //                client.SendMessage(chatId, "Hello World!"); // Send a message
        //            }
        //            var offset = updates.Last().UpdateId + 1;
        //            updates = client.GetUpdates(offset);
        //        }
        //        else
        //        {
        //            updates = client.GetUpdates();
        //        }
        //    }
        //    return Ok();
        //}
    }
}
