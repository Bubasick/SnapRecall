using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Messages.Interfaces
{
    public interface IMessage
    {
        public Task OnMessage(Message message, CancellationToken cancellationToken);
    }
}
