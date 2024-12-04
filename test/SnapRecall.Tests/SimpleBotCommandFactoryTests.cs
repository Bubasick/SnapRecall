using MediatR;
using Moq;
using SnapRecall.Application.Commands;
using SnapRecall.Application.Commands.Interfaces;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;

namespace SnapRecall.Tests
{
    public class SimpleBotCommandFactoryTests
    {
        private readonly Mock<SnapRecallDbContext> _dbContextMock;
        private readonly Mock<ITelegramBotClient> _clientMock;
        private readonly Mock<IMediator> _mediatorMock;

        public SimpleBotCommandFactoryTests()
        {
            _dbContextMock = new Mock<SnapRecallDbContext>();
            _clientMock = new Mock<ITelegramBotClient>();
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void GetCommandHandler_ReturnsConfirmTopicCreationBotCommand()
        {
            var commandName = BotCommands.ConfirmTopicCreationBotCommand;
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<ConfirmTopicCreationBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsStartBotCommand()
        {
            var commandName = BotCommands.StartBotCommand;
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<StartBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsViewTopicsBotCommand()
        {
            var commandName = BotCommands.ViewTopicsBotCommand;
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<ViewTopicsBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsNewTopicBotCommand()
        {
            var commandName = BotCommands.NewTopicBotCommand;
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<NewTopicBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsBeginTopicBotCommand_WhenCommandNameStartsWithBegin()
        {
            var commandName = BotCommands.BeginTopicBotCommand + "Extra";
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<BeginTopicBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsDeleteTopicBotCommand_WhenCommandNameStartsWithDelete()
        {
            var commandName = BotCommands.DeleteTopicBotCommand + "Extra";
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<DeleteTopicBotCommand>(result);
        }

        [Fact]
        public void GetCommandHandler_ReturnsNonExistentCommand_ForUnknownCommand()
        {
            var commandName = "UnknownCommand";
            var result = SimpleBotCommandFactory.GetCommandHandler(
                _dbContextMock.Object,
                _clientMock.Object,
                _mediatorMock.Object,
                commandName);

            Assert.IsType<NonExistentCommand>(result);
        }
    }
}