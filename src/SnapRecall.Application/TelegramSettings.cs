namespace SnapRecall.Application;

public class TelegramSettings
{
    public const string SectionName = "Telegram";

    public string BotToken { get; init; } = null!;
}