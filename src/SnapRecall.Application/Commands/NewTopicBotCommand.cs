using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Application.Features.Topics.CreateTopicCommand;
using SnapRecall.Application.Features.Users.UpdateUserCommand;

using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands;

public class NewTopicBotCommand(ISnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = Commands.Interfaces.BotCommands.NewTopicBotCommand;
    public static string Description = "add new topic";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        var topic = dbContext.Topics
            .Include(x => x.Author)
            .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

        if (topic != null)
        {
            await client.SendMessageAsync(message.Chat.Id, "You have an unfinished topic", cancellationToken: cancellationToken);
            return;
        }

        await mediator.Send(
            new UpdateUserCommand()
            {
                Id = message.From.Id,
                Name = message.From.FirstName,
                LastExecutedCommand = NewTopicBotCommand.Name,
                Tag = message.From.Username,
            },
            cancellationToken);

        await mediator.Send(
            new CreateTopicCommand()
            {
                AuthorId = message.From.Id,
            },
            cancellationToken);


        await client.SendMessageAsync(message.Chat.Id, "Please enter topic name", cancellationToken: cancellationToken);
    }
}