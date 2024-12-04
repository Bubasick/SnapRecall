using MediatR;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Application.Features.Users.CreateUserCommand;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands;

public class StartBotCommand(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public static string Name = BotCommands.StartBotCommand;
    public static string Description = "";

    public static BotCommand Command => new(Name, Description);

    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        await client.SendMessageAsync(message.Chat.Id, "This bot will help you create a quiz with a series of multiple choice questions.", cancellationToken: cancellationToken);
        if (!string.IsNullOrEmpty(previousCommand))
        {
            return;
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
    }
}