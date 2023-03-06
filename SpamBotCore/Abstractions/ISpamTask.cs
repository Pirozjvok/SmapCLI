namespace SpamBotCore.Abstractions
{
    public interface ISpamTask
    {
        public Task Do(CancellationToken token);
    }
}