using BlazorWasm.Client.Proxies;
using BlazorWasm.Shared.Interfaces;
using BlazorWasm.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BlazorWasm.Client.Pages
{
    public partial class EchoService : IMessageClient
    {
        private List<MessageModel> _messages = new();
        private IMessageHub _hubProxy = default;
        private HubConnection _connection = default;

        private string _senderGuid { get; set; } = "";

        private string MessageToAdd { get; set; } = "";

        //private string messagesMarkup { get; set; }

        private RenderFragment messagesRenderFragment { get; set; }

        private string svgHeight = "900px";

        [Inject]
        IWebAssemblyHostEnvironment Environment { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(Environment.IsProduction() ? "https://localhost:7189/messagehub" : "https://localhost:7189/messagehub")
                .Build();
            _hubProxy = _connection.ServerProxy<IMessageHub>();
            _ = _connection.ClientRegistration<IMessageClient>(this);
            await _connection.StartAsync();

            _messages = await _hubProxy.LoadMessages();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                if (_senderGuid == "") _senderGuid = user.Claims.First(claim => claim.Type.Contains("sid"))?.Value;
            }

            SetupMessagesForRendering();
        }

        private void Render_Prepare()
        {

        }
        private void SetupMessagesForRendering()
        {
            int iOtherSenderOffset = 100;

            int iY = 10;
            int iWidth_px = 750;
            int iHeight_px = 70;
            DateTime lastDate = DateTime.MinValue.Date;

            messagesRenderFragment = builder =>
            {
                int iSequence = 0;
                string sSvgHeight = svgHeight;
                foreach (MessageModel message in _messages)
                {
                    if (message != null)
                    {
                        int iX = (message.SenderGuid != _senderGuid ? 0 : iOtherSenderOffset);
                        string sBlockColor = (message.SenderGuid != _senderGuid ? "#DCDCDC" : "#D9FDD3");

                        #region Date

                        if (lastDate != message.DateTime.Date)
                        {
                            int iX_Date = ((iOtherSenderOffset + iWidth_px) / 2) - 50;

                            #region Containing block
                            //sbMessagesMarkup.AppendLine($"<rect fill=#EBEAEA x={iX_Date.ToString()} y={iY.ToString()} width='{100}px' height='{25}px' />");
                            builder.OpenElement(++iSequence, "rect");
                            builder.AddAttribute(++iSequence, "fill", "#EBEAEA");
                            builder.AddAttribute(++iSequence, "x", iX_Date.ToString());
                            builder.AddAttribute(++iSequence, "y", iY.ToString());
                            builder.AddAttribute(++iSequence, "width", $"100px");
                            builder.AddAttribute(++iSequence, "height", $"25px");
                            builder.CloseElement();
                            #endregion Containing block
                            #region Text
                            //sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX_Date + 10).ToString()}' y='{(iY + 2).ToString()}' width = '90px' height = '22px' >");
                            //sbMessagesMarkup.AppendLine($"<label><strong>{message.DateTime.ToString("yyyy/MM/dd")}</strong></label>");
                            //sbMessagesMarkup.AppendLine($"</foreignObject>");
                            builder.OpenElement(++iSequence, "foreignObject");
                            builder.AddAttribute(++iSequence, "x", (iX_Date + 10).ToString());
                            builder.AddAttribute(++iSequence, "y", (iY + 2).ToString());
                            builder.AddAttribute(++iSequence, "width", $"90px");
                            builder.AddAttribute(++iSequence, "height", $"22px");
                            builder.OpenElement(++iSequence, "label");
                            builder.OpenElement(++iSequence, "strong");
                            builder.AddContent(++iSequence, $"{message.DateTime.ToString("yyyy/MM/dd")}");
                            builder.CloseElement(); //strong
                            builder.CloseElement(); //label
                            builder.CloseElement(); //foreignObject
                            #endregion Text

                            lastDate = message.DateTime.Date;
                            iY += 30;
                        }

                        #endregion Date
                        #region Message

                        #region Containing block

                        //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={iX.ToString()} y={iY.ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
                        builder.OpenElement(++iSequence, "rect");
                        builder.AddAttribute(++iSequence, "fill", sBlockColor);
                        builder.AddAttribute(++iSequence, "x", iX.ToString());
                        builder.AddAttribute(++iSequence, "y", iY.ToString());
                        builder.AddAttribute(++iSequence, "width", $"{iWidth_px.ToString()}px");
                        builder.AddAttribute(++iSequence, "height", $"{iHeight_px.ToString()}px");
                        builder.AddAttribute(++iSequence, "filter", "url(#shadow)");
                        builder.CloseElement(); //rect

                        #endregion Containing block
                        #region Close button

                        //sbMessagesMarkup.AppendLine($"<text @onclick='() => Click()' x={(iX + iWidth_px - 27).ToString()} y={(iY + 2).ToString()} alignment-baseline='before-edge' class='note-markers' style='pointer-events:inherit'>❌</text>");
                        //sbMessagesMarkup.AppendLine($"<text @onclick='() => Click()' style='pointer-events:inherit'>❌</text>");
                        builder.OpenElement(++iSequence, "text");
                        builder.AddAttribute(++iSequence, "x", (iX + iWidth_px - 27).ToString());
                        builder.AddAttribute(++iSequence, "y", (iY + 2).ToString());
                        builder.AddAttribute(++iSequence, "alignment-baseline", "before-edge");
                        builder.AddAttribute(++iSequence, "class", "note-markers");
                        builder.AddAttribute(++iSequence, "style", "pointer-events:inherit");
                        builder.AddAttribute(++iSequence, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, () => ButtonClick_DeleteMessage(message.Id)));
                        builder.AddContent(++iSequence, "❌");
                        builder.CloseElement(); //text

                        #endregion Close button
                        #region Text
                        //sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX + 5).ToString()}' y='{(iY + 25).ToString()}' width = '{(iWidth_px - 10).ToString()}px' height = '{(iHeight_px - 30).ToString()}px' >");
                        //sbMessagesMarkup.AppendLine($"<label>{message.Text}</label>");
                        //sbMessagesMarkup.AppendLine($"</foreignObject>");
                        builder.OpenElement(++iSequence, "foreignObject");
                        builder.AddAttribute(++iSequence, "x", (iX + 5).ToString());
                        builder.AddAttribute(++iSequence, "y", (iY + 25).ToString());
                        builder.AddAttribute(++iSequence, "width", $"{(iWidth_px > 30 ? iWidth_px - 30 : 30).ToString()}px");
                        builder.AddAttribute(++iSequence, "height", $"{(25).ToString()}px");
                        builder.OpenElement(++iSequence, "label");
                        builder.AddContent(++iSequence, message.Text);
                        builder.CloseElement(); //label
                        builder.CloseElement(); //foreignObject
                        #endregion Text
                        #region Sender
                        builder.OpenElement(++iSequence, "foreignObject");
                        builder.AddAttribute(++iSequence, "x", (iX + 5).ToString());
                        builder.AddAttribute(++iSequence, "y", (iY + iHeight_px - 25).ToString());
                        builder.AddAttribute(++iSequence, "width", $"{(iWidth_px > 150 ? iWidth_px-150 : 100).ToString()}px");
                        builder.AddAttribute(++iSequence, "height", $"25px");
                        builder.OpenElement(++iSequence, "label");
                        builder.AddAttribute(++iSequence, "class", "note-time");
                        builder.AddContent(++iSequence, (message.SenderGuid != _senderGuid ? message.Sender : "You"));
                        builder.CloseElement(); //label
                        builder.CloseElement(); //foreignObject
                        #endregion Sender
                        #region Date/Time
                        //    sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX + iWidth_px - 30).ToString()}' y='{(iY + iHeight_px - 25).ToString()}' width = '{80}px' height = '{25}px' >");
                        //    sbMessagesMarkup.AppendLine($"<label class='note-time'>{message.DateTime.ToString("HH:mm")}</label>");
                        //    sbMessagesMarkup.AppendLine($"</foreignObject>");
                        builder.OpenElement(++iSequence, "foreignObject");
                        builder.AddAttribute(++iSequence, "x", (iX + iWidth_px - 30).ToString());
                        builder.AddAttribute(++iSequence, "y", (iY + iHeight_px - 25).ToString());
                        builder.AddAttribute(++iSequence, "width", $"30px");
                        builder.AddAttribute(++iSequence, "height", $"25px");
                        builder.OpenElement(++iSequence, "label");
                        builder.AddAttribute(++iSequence, "class", "note-time");
                        builder.AddContent(++iSequence, message.DateTime.ToString("HH:mm"));
                        builder.CloseElement(); //label
                        builder.CloseElement(); //foreignObject
                        #endregion Date/Time

                        #endregion Message

                        iY += (iHeight_px + 10);
                        sSvgHeight = $"{(iY + 100).ToString()}px";
                    }
                }

                svgHeight = sSvgHeight;
            };

            //StringBuilder sbMessagesMarkup = new StringBuilder();

            //int iOtherSenderOffset = 50;

            //int iY = 10;
            //int iWidth_px = 200;
            //int iHeight_px = 100;
            //DateTime lastDate = DateTime.MinValue.Date;
            //foreach (MessageModel message in _messages)
            //{
            //    int iX = (message.SenderGuid == _senderGuid ? 0 : iOtherSenderOffset);

            //    if (lastDate != message.DateTime.Date)
            //    {
            //        int iX_Date = ((iOtherSenderOffset + iWidth_px) / 2) - 50;

            //        //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={iX_Date.ToString()} y={iY.ToString()} width='100px' height='25x' />");
            //        sbMessagesMarkup.AppendLine($"<rect fill=#EBEAEA x={iX_Date.ToString()} y={iY.ToString()} width='{100}px' height='{25}px' />");

            //        sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX_Date + 10).ToString()}' y='{(iY + 2).ToString()}' width = '90px' height = '22px' >");
            //        //sbMessagesMarkup.AppendLine($"<textarea @bind=message.Text class=\"note-textarea\"></textarea>");
            //        sbMessagesMarkup.AppendLine($"<label><strong>{message.DateTime.ToString("yyyy/MM/dd")}</strong></label>");
            //        sbMessagesMarkup.AppendLine($"</foreignObject>");

            //        lastDate = message.DateTime.Date;
            //        iY += 30;
            //    }

            //    //Containing block
            //    sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={iX.ToString()} y={iY.ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");

            //    //Close button
            //    //sbMessagesMarkup.AppendLine($"<text>");
            //    sbMessagesMarkup.AppendLine($"<text @onclick='() => Click()' x={(iX + iWidth_px - 27).ToString()} y={(iY + 2).ToString()} alignment-baseline='before-edge' class='note-markers' style='pointer-events:inherit'>❌</text>");
            //    //sbMessagesMarkup.AppendLine($"</text>");

            //    //Text
            //    sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX + 5).ToString()}' y='{(iY + 25).ToString()}' width = '{(iWidth_px - 10).ToString()}px' height = '{(iHeight_px - 30).ToString()}px' >");
            //    //sbMessagesMarkup.AppendLine($"<textarea @bind=message.Text class=\"note-textarea\"></textarea>");
            //    sbMessagesMarkup.AppendLine($"<label>{message.Text}</label>");
            //    sbMessagesMarkup.AppendLine($"</foreignObject>");

            //    //Date/Time
            //    sbMessagesMarkup.AppendLine($"<foreignObject x='{(iX + iWidth_px - 30).ToString()}' y='{(iY + iHeight_px - 25).ToString()}' width = '{80}px' height = '{25}px' >");
            //    //sbMessagesMarkup.AppendLine($"<textarea @bind=message.Text class=\"note-textarea\"></textarea>");
            //    sbMessagesMarkup.AppendLine($"<label class='note-time'>{message.DateTime.ToString("HH:mm")}</label>");
            //    sbMessagesMarkup.AppendLine($"</foreignObject>");

            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + iWidth_px + 10).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (2 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (3 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (4 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (5 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (6 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");
            //    //sbMessagesMarkup.AppendLine($"<rect fill=#D9FDD3 x={(iX + (7 * (iWidth_px + 10))).ToString()} y={(iY + 50).ToString()} width='{iWidth_px.ToString()}px' height='{iHeight_px.ToString()}px' filter='url(#shadow)' />");

            //    iY += (iHeight_px + 10);
            //    svgHeight = $"{(iY + 50).ToString()}px";
            //}

            //messagesMarkup  = sbMessagesMarkup.ToString();
        }

        public Task MessageCreated(MessageModel message)
        {
            _messages.Add(message);
            MessageToAdd = "";
            SetupMessagesForRendering();
            StateHasChanged();

            return Task.CompletedTask;
        }

        public Task MessageDeleted(Guid id)
        {
            if (_messages.FirstOrDefault(message => message.Id == id) is not { } localMessage)
                return Task.CompletedTask;

            _messages.Remove(localMessage);
            MessageToAdd = "";
            SetupMessagesForRendering();
            StateHasChanged();

            return Task.CompletedTask;
        }

        private async void ButtonClick_Send()
        {
            if (MessageToAdd.Length > 0)
            {
                if (MessageToAdd.Length > 80) MessageToAdd = MessageToAdd.Substring(0, 80);

                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    if (_senderGuid == "") _senderGuid = user.Claims.First(claim => claim.Type.Contains("sid"))?.Value;

                    await _hubProxy.CreateMessage(user.Identity.Name, _senderGuid, MessageToAdd);
                }

                MessageToAdd = "";
            }
        }

        private async void ButtonClick_Clear()
        {
            await _hubProxy.ClearMessages();

            MessageToAdd = "";
        }

        private async Task ButtonClick_DeleteMessage(Guid messageId)
        {
            await _hubProxy.DeleteMessage(messageId);

            MessageToAdd = "";
        }
    }
}