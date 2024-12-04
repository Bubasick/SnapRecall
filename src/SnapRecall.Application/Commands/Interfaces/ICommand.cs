using Telegram.BotAPI.AvailableTypes;

namespace SnapRecall.Application.Commands.Interfaces
{
    public interface ICommand
    {
        public Task OnCommand(string previousCommand, string command, Message message, CancellationToken cancellationToken);
    }
}
