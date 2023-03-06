using SpamBotCore;
using SpamBotCore.SpamTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.RequestParams;

namespace SpamBot
{
    internal class SuperSpam
    {
        public List<VkApi> VkApis { get; set; } = new List<VkApi>();
        public List<string> Messages { get; set; } = new List<string>();
        public List<uint> UserIds { get; set; } = new List<uint>();
        public SpamManager SpamMgr { get; private set; }
        public TimeSpan DelayInterval { get; set; }
        public TimeSpan UserInterval { get; set; }

        public SuperSpam()
        {
            SpamMgr = new SpamManager();
            DelayInterval = TimeSpan.FromSeconds(10);
            UserInterval = TimeSpan.FromSeconds(10);
        }
        public async Task Start(CancellationToken token)
        {
            Task workers = SpamMgr.Start(token);

            while (!token.IsCancellationRequested)
            {
                foreach (uint user_id in UserIds)
                    foreach (string msg in Messages)
                    {
                        foreach (VkApi api in VkApis)
                        {
                            MessagesSendParams sendParams = new MessagesSendParams();
                            sendParams.UserId = user_id;
                            sendParams.Message = msg;
                            sendParams.RandomId = Random.Shared.Next();
                            MessageSpamTask message = new MessageSpamTask(api, sendParams);
                            SpamMgr.AddTask(message);
                        }
                        await Task.Delay(UserInterval);
                    }
                        

                await Task.Delay(DelayInterval);
            }

            await workers;
        }
    }
}
