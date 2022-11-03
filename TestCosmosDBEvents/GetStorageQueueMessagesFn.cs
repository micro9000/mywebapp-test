using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCosmosDBEvents
{
    public class GetStorageQueueMessagesFn
    {
        private readonly ILogger _logger;

        public GetStorageQueueMessagesFn(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCosmosDbEventsFn>();
        }

        [Function(nameof(GetStorageQueueMessagesFn))]
        [QueueOutput("output-queue")]
        public static string[] Run([QueueTrigger("input-queue")] Book myQueueItem, FunctionContext context)
        {
            // Use a string array to return more than one message.
            string[] messages = {
                    $"Book name = {myQueueItem.Name}",
                    $"Book ID = {myQueueItem.Id}"
            };

            var logger = context.GetLogger("QueueFunction");
            logger.LogInformation($"{messages[0]},{messages[1]}");

            // Queue Output messages
            return messages;
        }
    }

    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }

}
