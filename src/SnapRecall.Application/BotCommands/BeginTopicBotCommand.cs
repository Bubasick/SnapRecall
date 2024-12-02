using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.BotCommands;
using SnapRecall.Application.Features.Users.UpdateUserCommand;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public class BeginTopicBotCommand(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = Commands.BeginTopicBotCommand;
    public static string Description = "begin topic";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        int.TryParse(command.TrimStart(Commands.BeginTopicBotCommand.ToCharArray()), CultureInfo.InvariantCulture, out var id);

        var questions = dbContext.Quizzes
            .Include(x => x.Options)
            .Include(x => x.Topic)
            .Where(x => x.TopicId == id && x.Topic.IsCreationFinished)
            .ToList();

        if (questions.Count == 0)
        {
            await client.SendMessageAsync(message.Chat.Id, "topic not found", cancellationToken: cancellationToken);
            return;
        }

        await mediator.Send(
            new UpdateUserCommand()
            {
                Id = message.From.Id,
                Name = message.From.FirstName,
                LastExecutedCommand = BeginTopicBotCommand.Name,
                Tag = message.From.Username,
            },
            cancellationToken);

        foreach (var question in questions)
        {
            var options = question.Options.Select(x => new InputPollOption() { Text = x.Text }).ToList();
            var correctOptionId = question.Options.FindIndex(0, x => x.IsCorrect);
            await client.SendPollAsync(message.Chat.Id, question.Text, options, type: "quiz", correctOptionId: correctOptionId, cancellationToken: cancellationToken);
        }
    }
}