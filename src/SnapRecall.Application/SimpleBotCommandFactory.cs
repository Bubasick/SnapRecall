using MediatR;
using SnapRecall.Application.BotCommands;
using SnapRecall.Domain.BotCommands;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;

namespace SnapRecall.Application
{
    public static class SimpleBotCommandFactory
    {
        public static ICommand GetCommandHandler(SnapRecallDbContext dbContext, ITelegramBotClient client, IMediator mediator, string commandName)
        {
            ICommand handler;
            if (commandName == Commands.ConfirmTopicCreationBotCommand)
            {
                handler = new ConfirmTopicCreationBotCommand(dbContext, client, mediator);
            }
            else if (commandName == Commands.StartBotCommand)
            {
                handler = new StartBotCommand(dbContext, client, mediator);
            }
            else if (commandName == Commands.ViewTopicsBotCommand)
            {
                handler = new ViewTopicsBotCommand(dbContext, client, mediator);
            }
            else if (commandName == Commands.NewTopicBotCommand)
            {
                handler = new NewTopicBotCommand(dbContext, client, mediator);
            }
            else if (commandName.StartsWith(Commands.BeginTopicBotCommand))
            {
                handler = new BeginTopicBotCommand(dbContext, client, mediator);
            }
            else if (commandName.StartsWith(Commands.DeleteTopicBotCommand))
            {
                handler = new DeleteTopicBotCommand(dbContext, client, mediator);
            }
            else
            {
                handler = new NonExistentCommand(dbContext, client, mediator);
            }

            return handler;
        }
    }
}
