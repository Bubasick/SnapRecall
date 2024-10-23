using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public static class TopicBotCommand
{
    public const string Name = "topics";
    public const string Description = "shows your topics";

    public static BotCommand Command => new(Name, Description);
}