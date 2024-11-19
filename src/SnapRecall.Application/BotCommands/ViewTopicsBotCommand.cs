using System.Text;
using MediatR;
using SnapRecall.Application.BotCommands;
using SnapRecall.Application.Features.Users.UpdateUserCommand;
using SnapRecall.Domain.Features.Topics.GetTopicsRequest;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public class ViewTopicsBotCommand(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = Commands.ViewTopicsBotCommand;
    public static string Description = "view your topics";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        if (previousCommand == NewTopicBotCommand.Name)
        {
            await client.SendMessageAsync(message.Chat.Id, "Please, finish topic creation with /done first",
                cancellationToken: cancellationToken);
        }

        var topics = await mediator.Send(
            new GetTopicsRequest()
            {
                UserId = message.From.Id,
            },
            cancellationToken);

        await mediator.Send(
            new UpdateUserCommand()
            {
                Id = message.From.Id,
                Name = message.From.FirstName,
                LastExecutedCommand = ViewTopicsBotCommand.Name,
                Tag = message.From.Username,
            },
            cancellationToken);

        var sb = new StringBuilder();
        foreach (var tp in topics)
        {
            sb.AppendLine($"{tp.Name} - /begin{tp.Id}");
            
        }

        await client.SendMessageAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
    }
}