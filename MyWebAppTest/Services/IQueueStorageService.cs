namespace MyWebAppTest.Services
{
    public interface IQueueStorageService<T> where T : class
    {
        void InsertMessage(string queueName, T message);
        Task InsertMessageASync(string queueName, T message);
        T? PeekMessage(string queueName);
        void UpdateMessage(string queueName, T message);
        IEnumerable<T?> DequeueMessage(string queueName);
    }
}
