using SpamBot;
using SpamBotCore;
using SpamBotCore.SpamTasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

SuperSpam superSpam = new SuperSpam();

VkApi first = new VkApi();
first.Authorize(new ApiAuthParams
{
    ApplicationId = 2685278,
    Login = "Login",
    Password = "Password",
    Settings = Settings.All,
    ClientSecret = "hHbJug59sKJie78wjrH8"
});


superSpam.Messages.Add("Pizdec");
superSpam.Messages.Add("Uzas");
superSpam.Messages.Add("Govno");
superSpam.Messages.Add("Php");

superSpam.VkApis.Add(first);

superSpam.UserIds.Add(000000000);

superSpam.DelayInterval = TimeSpan.FromSeconds(0);
superSpam.UserInterval = TimeSpan.FromSeconds(1);

superSpam.SpamMgr.SetWorkersCount(3);

CancellationTokenSource cts = new CancellationTokenSource();

Task work = superSpam.Start(cts.Token);

Console.WriteLine("Spam Started");
Console.WriteLine("Please press enter to stop");

Console.ReadLine();
Console.WriteLine("Stoping...");
cts.Cancel();

try
{
    work.Wait();
}
catch (AggregateException e) when (e.InnerException is OperationCanceledException)
{

}