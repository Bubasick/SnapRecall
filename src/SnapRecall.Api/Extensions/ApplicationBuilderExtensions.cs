using SnapRecall.Application.SnapRecallBotHandlers;
using SnapRecall.Domain.BotCommands;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Registers the Telegram Webhook.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance this method extends.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseTelegramWebhook(this IApplicationBuilder app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var bot = app.ApplicationServices.GetRequiredService<ITelegramBotClient>();

        var webhookToken = configuration["Telegram:WebhookToken"]; // ENV: Telegram__WebhookToken, JSON: "Telegram:WebhookToken"
        var webhookUrl = configuration["Telegram:WebhookUrl"]; // ENV: Telegram__WebhookUrl, JSON: "Telegram:WebhookUrl"

        // Delete my old commands
        bot.DeleteMyCommands();
        // Set my commands
        bot.SetMyCommands([NewTopicBotCommand.Command, ViewTopicsBotCommand.Command]);

        // Delete webhook
        bot.DeleteWebhook();

        // Set webhook
        bot.SetWebhook("https://working-badger-model.ngrok-free.app/bot", secretToken: "webhookToken");

        return app;
    }
}