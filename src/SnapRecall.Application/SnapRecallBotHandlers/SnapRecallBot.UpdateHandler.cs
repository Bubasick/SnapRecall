﻿using Microsoft.Extensions.Logging;
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

            await base.OnUpdateAsync(update, cancellationToken);
        }
    }
}
