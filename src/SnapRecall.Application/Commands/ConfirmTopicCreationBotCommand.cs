using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Application.Features.Topics.UpdateTopicCommand;
using SnapRecall.Application.Features.Users.UpdateUserCommand;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands;

public class ConfirmTopicCreationBotCommand(
    SnapRecallDbContext dbContext,
    ITelegramBotClient client,
    ISender mediator) : ICommand
{
    public static string Name = BotCommands.ConfirmTopicCreationBotCommand;
    public static string Description = "confirm topic creation";
        
    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        if (previousCommand != NewTopicBotCommand.Name)
        {
            return;
        }

        var unfinishedTopic = dbContext.Topics
            .Include(x => x.Author)
            .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

        if (unfinishedTopic is null)
        {
            await client.SendMessageAsync(message.Chat.Id, "You are not in the process of creating a topic",
                cancellationToken: cancellationToken);
        }

        await mediator.Send(
            new UpdateTopicCommand()
            {
                TopicId = unfinishedTopic.Id,
                Name = unfinishedTopic.Name,
                Description = unfinishedTopic.Description,
                IsCreationFinished = true,
            },
            cancellationToken);

        await mediator.Send(
            new UpdateUserCommand()
            {
                Id = message.From.Id,
                Name = message.From.FirstName,
                LastExecutedCommand = Name,
                Tag = message.From.Username,
            },
            cancellationToken);

        await client.SendMessageAsync(message.Chat.Id, "Finished topic creation",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
    }
}