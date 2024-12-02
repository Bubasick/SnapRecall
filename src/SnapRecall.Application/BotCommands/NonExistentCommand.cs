﻿using MediatR;
using SnapRecall.Application.BotCommands;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Domain.BotCommands;

public class NonExistentCommand(SnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : ICommand
{
    public async Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken)
    {
        await client.SendMessageAsync(message.Chat.Id, "This command does not exist", cancellationToken: cancellationToken);
    }
}