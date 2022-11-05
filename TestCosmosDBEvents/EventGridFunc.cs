using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestCosmosDBEvents
{
    public class EventGridFunc
    {
        private readonly ILogger _logger;

        public EventGridFunc(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCosmosDbEventsFn>();
        }

        [Function(nameof(EventGridFunc))]
        public void Run([EventGridTrigger] MyEvent input)
        {
            _logger.LogInformation(input.Data.ToString());
        }
    }

    public class MyEvent
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Subject { get; set; }

        public string EventType { get; set; }

        public DateTime EventTime { get; set; }

        public object Data { get; set; }
    }
}
