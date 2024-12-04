using Microsoft.Extensions.Logging;
using SnapRecall.Application.Commands.Interfaces;
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
            var previousCommand = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id)?.LastExecutedCommand;

            var handler = SimpleBotCommandFactory.GetCommandHandler(dbContext, client, mediator, commandName);

            await handler.OnCommand(previousCommand, commandName, message, cancellationToken);
        }
    }
}
