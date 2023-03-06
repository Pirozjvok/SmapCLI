using SpamBotCore.Abstractions;
using VkNet;
using VkNet.Model.RequestParams;

namespace SpamBotCore.SpamTasks
{
    public class MessageSpamTask : ISpamTask
    {
        public VkApi Api { get; init; }
        public MessagesSendParams SendParams { get; set; }
        public MessageSpamTask(VkApi vkApi, MessagesSendParams sendParams)
        {
            Api = vkApi;
            SendParams = sendParams;
        }
        public async Task Do(CancellationToken token)
        {
            await Api.Messages.SendAsync(SendParams);
        }
    }
}