using BlazorWasm.Shared.Models;

namespace BlazorWasm.Shared.Interfaces
{
    public interface IMessageHub
    {
        Task<List<MessageModel>> LoadMessages();
        Task CreateMessage(string sender, string senderGuid, string text);
        Task DeleteMessage(Guid id);
        Task ClearMessages();
    }
}
