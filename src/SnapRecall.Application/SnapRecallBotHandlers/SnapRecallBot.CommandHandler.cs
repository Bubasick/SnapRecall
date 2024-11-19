using Microsoft.Extensions.Logging;
using SnapRecall.Application.BotCommands;
using SnapRecall.Domain.BotCommands;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;

namespace SnapRecall.Application.SnapRecallBotHandlers
{
    public partial class SnapRecallBot : SimpleTelegramBotBase
    {
        protected override async Task OnCommandAsync(
            Message message,
            string commandName,
            string commandParameters,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var args = commandParameters.Split(' ');
#if DEBUG
            this.logger.LogInformation("Params: {ArgsLenght}", args.Length);
#endif
            var lastExecutedCommand = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id)?.LastExecutedCommand;
            if (commandName == Commands.ConfirmTopicCreationBotCommand)
            {
                var handler = new ConfirmTopicCreationBotCommand(dbContext, Client, mediator);
                await handler.OnCommand(lastExecutedCommand, commandName, message, cancellationToken);
            }
            else if (commandName == Commands.StartBotCommand)
            {
                var handler = new StartBotCommand(dbContext, Client, mediator);
                await handler.OnCommand(lastExecutedCommand, commandName, message, cancellationToken);
            }
            else if (commandName == Commands.ViewTopicsBotCommand)
            {
                var handler = new ViewTopicsBotCommand(dbContext, Client, mediator);
                await handler.OnCommand(lastExecutedCommand, commandName, message, cancellationToken);
            }
            else if (commandName == Commands.NewTopicBotCommand)
            {
                var handler = new NewTopicBotCommand(dbContext, Client, mediator);
                await handler.OnCommand(lastExecutedCommand, commandName, message, cancellationToken);
            }
            else if (commandName.StartsWith(Commands.BeginTopicBotCommand))
            {
                var handler = new BeginTopicBotCommand(dbContext, Client, mediator);
                await handler.OnCommand(lastExecutedCommand, commandName, message, cancellationToken);
            }
        }
    }
}
