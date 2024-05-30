using BlazorWasm.Shared.Models;

namespace BlazorWasm.Shared.Interfaces
{
    public interface IMessageClient
    {
        Task MessageCreated(MessageModel message);
        Task MessageDeleted(Guid id);
    }
}
