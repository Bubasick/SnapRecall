using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public static class StartBotCommand
{
    public const string Name = "start";
    public const string Description = "";

    public static BotCommand Command => new(Name, Description);
}