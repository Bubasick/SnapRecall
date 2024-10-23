using Microsoft.Extensions.Logging;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI;

namespace SnapRecall.Application.SnapRecallBotHandlers;

public partial class SnapRecallBot : SimpleTelegramBotBase
{
    protected override void OnBotException(BotRequestException exp)
    {
        this.logger.LogError("BotRequestException: {Message}", exp.Message);
    }

    protected override void OnException(Exception exp)
    {
        this.logger.LogError("Exception: {Message}", exp.Message);
    }
}