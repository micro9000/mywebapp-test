using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyWebAppTest.Services
{
    public class QueueStorageService<T> : IQueueStorageService<T> where T : class
    {

        private readonly string _storageConnectionString;

        public QueueStorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration["StorageConnectionString"];
        }

        private QueueClient GetQueueClient(string queueName)
        {
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClientObj = new QueueClient(_storageConnectionString, queueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            // Create the queue
            queueClientObj.CreateIfNotExists();
            return queueClientObj;
        }


        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        public void InsertMessage(string queueName, T message)
        {
            var queueClient = this.GetQueueClient(queueName);

            // Send a message to the queue
            queueClient.SendMessage(JsonSerializer.Serialize(message));
        }

        //-------------------------------------------------
        // Insert a message asynchronous into a queue
        //-------------------------------------------------
        public async Task InsertMessageASync(string queueName, T message)
        {
            var queueClient = this.GetQueueClient(queueName);
            // Send a message to the queue
            await queueClient.SendMessageAsync(JsonSerializer.Serialize(message));
        }


        //-------------------------------------------------
        // Peek at a message in the queue
        //-------------------------------------------------
        public T? PeekMessage(string queueName)
        {
            var queueClient = this.GetQueueClient(queueName);

            // Peek at the next message
            PeekedMessage[] peekedMessage = queueClient.PeekMessages();

            return JsonSerializer.Deserialize<T>(peekedMessage[0].Body);
        }

        //-------------------------------------------------
        // Update an existing message in the queue
        //-------------------------------------------------
        public void UpdateMessage(string queueName, T message)
        {
            var queueClient = this.GetQueueClient(queueName);
            // Get the message from the queue
            QueueMessage[] recvMessage = queueClient.ReceiveMessages();

            // Update the message contents
            queueClient.UpdateMessage(recvMessage[0].MessageId,
                    recvMessage[0].PopReceipt,
                    JsonSerializer.Serialize(message),
                    TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
                );
        }


        //-------------------------------------------------
        // Process and remove a message from the queue
        //-------------------------------------------------
        public IEnumerable<T?> DequeueMessage(string queueName)
        {
            var queueClient = this.GetQueueClient(queueName);

            // Get the next message
            QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

            // Process (i.e. print) the message in less than 30 seconds
            Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

            // Delete the message
            queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);

            return retrievedMessage.Select(x => JsonSerializer.Deserialize<T>(x.Body));
        }

    }
}
