using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public static class ConfirmTopicCreationBotCommand
{
    public const string Name = "done";
    public const string Description = "Confirm topic creation";

    public static BotCommand Command => new(Name, Description);
}