using Microsoft.Extensions.Logging;
using System.Threading;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.GettingUpdates;

namespace SnapRecall.Application.SnapRecallBotHandlers
{
    public partial class SnapRecallBot : SimpleTelegramBotBase
    {
        public override async Task OnUpdateAsync(Update update, CancellationToken cancellationToken)
        {
#if DEBUG
            this.logger.LogInformation(
                "New update with id: {UpdateId}. Type: {UpdateType}",
                update.UpdateId,
                update.GetUpdateType()
            );
#endif
            var myUsername =  await Client.GetMeAsync(cancellationToken);
            SetCommandExtractor(myUsername.Username!);
            if (CommandExtractor!.HasCommand(update.Message))
            {
                var (commandName, args) = CommandExtractor.ExtractCommand(update.Message);
                await OnCommandAsync(update.Message, commandName, args, cancellationToken);
                return;
            }
            else
            {
                await OnMessageAsync(update.Message, cancellationToken);
                return;
            }

            base.OnUpdate(update);
        }
    }
}
