using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.Extensions;

namespace SnapRecall.Application.SnapRecallBotHandlers;

/// <summary>
/// It contains the main functionality of the telegram bot. <br />
/// The application creates a new instance of this class to process each update received.
/// </summary>
public partial class SnapRecallBot : SimpleTelegramBotBase
{
    private readonly ILogger<SnapRecallBot> logger;

    private readonly IMediator mediator;

    private readonly SnapRecallDbContext dbContext;

    private readonly TelegramSettings settings;
    private readonly HttpClient httpClient;

    private ITelegramBotClient client { get; }

    public SnapRecallBot(
        ILogger<SnapRecallBot> logger,
        IConfiguration configuration,
        ITelegramBotClient botClient,
        IMediator mediator,
        SnapRecallDbContext dbContext,
        IOptions<TelegramSettings> settings,
        HttpClient httpClient)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.dbContext = dbContext;

        // var botToken = configuration.GetValue<string>("Telegram:BotToken");
        this.client = botClient;
        this.httpClient = httpClient;

        this.settings = settings.Value;
        var myUsername = this.client.GetMe().Username!;
        // This will provide a better command filtering.
        this.SetCommandExtractor(myUsername, true);
    }
}
