using System.Collections.Concurrent;
using SpamBotCore.Abstractions;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace SpamBotCore
{
    public class SpamManager
    {
        private BlockingCollection<ISpamTask> _spamTask { get; set; }

        private List<Task> _tasks = new List<Task>();

        private int _workers = 0;
        public int WorkersCount { get => _workers; }
        public bool IsActive { get; private set; }
        public SpamManager()
        {
            _spamTask = new BlockingCollection<ISpamTask>();
        }
        public void AddTask(ISpamTask spamTask)
        {
            _spamTask.Add(spamTask);
        }
        public async Task Start(CancellationToken token)
        {
            if (IsActive) throw new InvalidOperationException("Уже запущено");
            try
            {
                IsActive = true;
                for (int i = 0; i < _workers; i++)
                {
                    _tasks.Add(Task.Run(() => Consume(token)));
                }
                await Task.WhenAll(_tasks);
            }
            finally
            {
                IsActive = false;
            }

        }

        public void SetWorkersCount(int count)
        {
            if (IsActive) throw new InvalidOperationException("Уже запущено");
            _workers = count;
        }
        private void Consume(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ISpamTask task = _spamTask.Take(token);
                if (token.IsCancellationRequested) return;
                task.Do(token);
            }
        }
    }
}