using BlazorWasm.Shared.Interfaces;
using BlazorWasm.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorWasm.Server
{
    public class MessageHub : Hub<IMessageClient>, IMessageHub
    {
        public Task<List<MessageModel>> LoadMessages()
        {
            return Task.FromResult(StaticStorage.Messages);
        }

        public async Task CreateMessage(string sender, string senderGuid, string text)
        {
            MessageModel newMessage = new MessageModel(sender, senderGuid, text);
            StaticStorage.Messages.Add(newMessage);

            await Clients.All.MessageCreated(newMessage);
        }

        public async Task DeleteMessage(Guid id)
        {
            if (StaticStorage.Messages.FirstOrDefault(message => message.Id == id) is not { } serverMessage)
                return;

            StaticStorage.Messages.Remove(serverMessage);
            await Clients.All.MessageDeleted(id);
        }

        public async Task ClearMessages()
        {
            List<Guid> messageIds = StaticStorage.Messages.Select(message => message.Id).ToList();
            foreach (Guid id in messageIds)
            {
                await DeleteMessage(id);
            }
        }
    }
}
