using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public static class NewTopicBotCommand
{
    public const string Name = "newtopic";
    public const string Description = "add new topic";

    public static BotCommand Command => new(Name, Description);
}