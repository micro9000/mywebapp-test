using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestCosmosDBEvents
{
    public class GetCosmosDbEventsFn
    {
        private readonly ILogger _logger;

        public GetCosmosDbEventsFn(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCosmosDbEventsFn>();
        }

        [Function(nameof(GetCosmosDbEventsFn))]
        public void Run([CosmosDBTrigger(
            databaseName: "MyDatabase",
            collectionName: "Accounts",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases")] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].Id);
            }
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
