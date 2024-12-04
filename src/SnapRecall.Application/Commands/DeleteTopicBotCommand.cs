using System.Globalization;
using MediatR;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Application.Features.Topics.DeleteTopicCommand;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands;

public class DeleteTopicBotCommand(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = BotCommands.DeleteTopicBotCommand;
    public static string Description = "";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        int.TryParse(command.TrimStart(Name.ToCharArray()), CultureInfo.InvariantCulture, out var topicId);

        await mediator.Send(
            new DeleteTopicCommand()
            {
                UserId = message.From.Id,
                TopicId = topicId,
            },
            cancellationToken);

        await client.SendMessageAsync(message.Chat.Id, "Topic was successfully deleted", cancellationToken: cancellationToken);
    }
}