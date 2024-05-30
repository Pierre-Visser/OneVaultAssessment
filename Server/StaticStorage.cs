using BlazorWasm.Shared.Models;

namespace BlazorWasm.Server
{
    public static class StaticStorage
    {
        public static List<MessageModel> Messages { get; set; } = new();
    }
}
