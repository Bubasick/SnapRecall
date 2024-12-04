using Microsoft.Extensions.Logging;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using SnapRecall.Application.Messages.Interfaces;

namespace SnapRecall.Application.SnapRecallBotHandlers
{
    public partial class SnapRecallBot : SimpleTelegramBotBase
    {
        protected override async Task OnMessageAsync(Message message, CancellationToken cancellationToken)
        {
            // Ignore user 777000 (Telegram)
            if (message.From?.Id == TelegramConstants.TelegramId)
            {
                return;
            }

            var myUsername = await client.GetMeAsync(cancellationToken);
            SetCommandExtractor(myUsername.Username!);
            if (CommandExtractor!.HasCommand(message))
            {
                var (commandName, args) = CommandExtractor.ExtractCommand(message);
                await OnCommandAsync(message, commandName, args, cancellationToken);
                return;
            }
#if DEBUG
            logger.LogInformation("New message from chat id: {ChatId}", message!.Chat.Id);
            logger.LogInformation("Message: {MessageContent}", message.Text);
#endif

            var user = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id);
            if (user is null)
            {
                return;
            }

            var handler = SimpleBotMessageFactory.GetMessageHandler(message, dbContext, client, mediator, settings, httpClient);

            await handler.OnMessage(message, cancellationToken);

            await base.OnMessageAsync(message, cancellationToken);
        }
    }
}
