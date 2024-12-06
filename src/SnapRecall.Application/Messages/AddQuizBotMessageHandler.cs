using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Features.Questions;
using SnapRecall.Application.Messages.Interfaces;
using SnapRecall.Domain;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages
{
    public class AddQuizBotMessageHandler(ISnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : IMessage
    {
        public async Task OnMessage(Message message, CancellationToken cancellationToken)
        {
            var unfinishedTopic = dbContext.Topics
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

            if (message.Poll != null && message.Poll.CorrectOptionId.HasValue)
            {
                var optionsToAdd = new List<Option>();
                var options = message.Poll.Options.ToList();
                for (var i = 0; i < options.Count(); i++)
                {
                    optionsToAdd.Add(new Option()
                    {
                        Text = options[i].Text,
                        IsCorrect = message.Poll.CorrectOptionId.Value == i,
                    });
                }

                await mediator.Send(new AddQuizCommand()
                {
                    TopicId = unfinishedTopic.Id,
                    Text = message.Poll.Question,
                    Options = optionsToAdd,

                }, cancellationToken);

                await RenderCreateQuestionButton(message, "Question added. Use /done to finish topic creation", cancellationToken);
            }
        }

        private async Task RenderCreateQuestionButton(Message message, string text = default, CancellationToken cancellationToken = default)
        {
            var keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Create quiz")
                    {
                        RequestPoll = new KeyboardButtonPollType{Type = "quiz"}

                    },
                    new KeyboardButton("Create question"),
                }
            };

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboard);

            keyboardMarkup.ResizeKeyboard = true;

            await client.SendMessageAsync(message.Chat.Id, text, replyMarkup: keyboardMarkup, cancellationToken: cancellationToken);
        }
    }
}
