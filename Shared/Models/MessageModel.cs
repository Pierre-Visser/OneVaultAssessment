namespace BlazorWasm.Shared.Models
{
    public class MessageModel
    {
        public MessageModel() { }
        public MessageModel(string sender, string senderGuid, string text)
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.Now;
            Text = text;
            Sender = sender;
            SenderGuid = senderGuid;
        }

        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Sender { get; set; }
        public string SenderGuid { get; set; }
        public DateTime DateTime { get; set; }
    }
}
