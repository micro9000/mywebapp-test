using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebAppTest.Models;
using MyWebAppTest.Services;

namespace MyWebAppTest.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly ITableStorageService _tableStorageService;

        public GroceryItemEntity itemOne;

        public PrivacyModel(ILogger<PrivacyModel> logger, ITableStorageService tableStorageService)
        {
            _logger = logger;
            _tableStorageService = tableStorageService;
        }


        public async Task OnGet()
        {
            string id = Guid.NewGuid().ToString();
            string category = "Test";

            var groceryItem = new GroceryItemEntity
            {
                Id = id,
                Name = "test",
                Category = category,
                Price = 123,
                PartitionKey = category,
                RowKey = id,
                ETag = Azure.ETag.All
            };

            await _tableStorageService.UpsertEntityAsync(groceryItem);

            itemOne = await _tableStorageService.GetEntityAsync(category, id);



            _logger.LogInformation("Log from Privacy page");
        }
    }
}