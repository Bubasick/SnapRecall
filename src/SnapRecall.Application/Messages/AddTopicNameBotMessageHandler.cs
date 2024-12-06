using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.Features.Topics.UpdateTopicCommand;
using SnapRecall.Application.Messages.Interfaces;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages
{
    public class AddTopicNameBotMessageHandler(ISnapRecallDbContext dbContext, ITelegramBotClient client, ISender mediator) : IMessage
    {
        public async Task OnMessage(Message message, CancellationToken cancellationToken)
        {
            var unfinishedTopic = dbContext.Topics
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Author.Id == message.From.Id && !x.IsCreationFinished);

            if (unfinishedTopic is null)
            {
                throw new Exception();
            }

            if (!string.IsNullOrEmpty(unfinishedTopic.Name))
            {
                await client.SendMessageAsync(message.Chat.Id, "Topic already named. Please add your first question using the button below", cancellationToken: cancellationToken);
                return;
            }

            if (!string.IsNullOrEmpty(message.Text))
            {
                await mediator.Send(new UpdateTopicCommand()
                {
                    TopicId = unfinishedTopic.Id,
                    Name = message.Text,
                });

                await RenderCreateQuestionButton(message, "Good. Now you can press the button below to add quiz. Or, you can type a 2-paragraph message (separated by blank line). First paragraph is the question (optional), second paragraph is the answer (optional). You can append attachments to message. Each message is treated as a separate question.", cancellationToken);
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
                    }
                }
            };

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboard);

            keyboardMarkup.ResizeKeyboard = true;

            await client.SendMessageAsync(message.Chat.Id, text, replyMarkup: keyboardMarkup, cancellationToken: cancellationToken);
        }
    }
}
