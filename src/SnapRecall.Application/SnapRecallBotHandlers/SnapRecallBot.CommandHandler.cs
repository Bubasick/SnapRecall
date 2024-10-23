using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnapRecall.Application.Features.Topics.CreateTopicCommand;
using SnapRecall.Application.Features.Topics.UpdateTopicCommand;
using SnapRecall.Application.Features.Users.CreateUserCommand;
using SnapRecall.Application.Features.Users.UpdateUserCommand;
using SnapRecall.Domain.BotCommands;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;

namespace SnapRecall.Application.SnapRecallBotHandlers
{
    public partial class SnapRecallBot : SimpleTelegramBotBase
    {
        protected override async Task OnCommandAsync(
            Message message,
            string commandName,
            string commandParameters,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var args = commandParameters.Split(' ');
#if DEBUG
            this.logger.LogInformation("Params: {ArgsLenght}", args.Length);
#endif
            var lastExecutedCommand = dbContext.Users.FirstOrDefault(x => x.Id == message.From.Id)?.LastExecutedCommand;   
            switch (commandName)
            {
                case StartBotCommand.Name:
                    this.Client.SendMessage(message.Chat.Id, "This bot will help you create a quiz with a series of multiple choice questions.");
                    if (!string.IsNullOrEmpty(lastExecutedCommand))
                    {
                        break;
                    }
                    await mediator.Send(
                        new CreateUserCommand()
                        {
                            Id = message.From.Id,
                            Name = message.From.FirstName,
                            LastExecutedCommand = StartBotCommand.Name,
                            Tag = message.From.Username,
                        }, 
                        cancellationToken);
                    break;

                case NewTopicBotCommand.Name:
                    var topic = dbContext.Topics
                        .Include(x => x.Author)
                        .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);
                    if (topic != null)
                    {
                        await Client.SendMessageAsync(message.Chat.Id, "You have an unfinished topic", cancellationToken: cancellationToken);
                        break;
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


                    this.Client.SendMessage(message.Chat.Id, "Please enter topic name");
                    break;

                case ConfirmTopicCreationBotCommand.Name: // Reply to /hello command
                    if (lastExecutedCommand == NewTopicBotCommand.Name)
                    {
                        var unfinishedTopic = dbContext.Topics
                            .Include(x => x.Author)
                            .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

                        if (unfinishedTopic is null)
                        {
                            this.Client.SendMessage(message.Chat.Id, "You are not in the process of creating a topic");
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
                                LastExecutedCommand = ConfirmTopicCreationBotCommand.Name,
                                Tag = message.From.Username,
                            },
                            cancellationToken);

                        this.Client.SendMessage(message.Chat.Id, "Finished topic creation");
                        break;
                    }
                    break;

                default:
                    //if (message.Chat.Type == ChatType.Private)
                    //{
                        this.Client.SendMessage(message.Chat.Id, "Unrecognized command.");
                    //}
                    break;
            }
        }
    }
}
