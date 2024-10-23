using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public static class AddQuestionBotCommand
{
    public const string Name = "addquestion";
    public const string Description = "add new question";

    public static BotCommand Command => new(Name, Description);
}