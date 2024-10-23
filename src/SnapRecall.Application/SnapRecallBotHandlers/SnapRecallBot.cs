using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public ITelegramBotClient Client { get; }

    public SnapRecallBot(ILogger<SnapRecallBot> logger, IConfiguration configuration, ITelegramBotClient botClient, IMediator mediator, SnapRecallDbContext dbContext)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.dbContext = dbContext;

        // var botToken = configuration.GetValue<string>("Telegram:BotToken");
        this.Client = botClient;

        var myUsername = this.Client.GetMe().Username!;
        // This will provide a better command filtering.
        this.SetCommandExtractor(myUsername, true);
    }
}
