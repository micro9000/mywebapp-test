using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebAppTest.Models;
using MyWebAppTest.Services;

namespace MyWebAppTest.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICosmosDbService cosmosDbService;
        private readonly IQueueStorageService<Item> queueStorageService;

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public IndexModel(ILogger<IndexModel> logger, ICosmosDbService cosmosDbService, IQueueStorageService<Item> queueStorageService)
        {
            _logger = logger;
            this.cosmosDbService = cosmosDbService;
            this.queueStorageService = queueStorageService;
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Log from index page");
            Items = await cosmosDbService.GetItemsAsync("SELECT * FROM c");
            //asdf
        }

        public void OnPost()
        {
            var name = Request.Form["name"];
            var description = Request.Form["description"];

            var newItem = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Completed = false
            };

            queueStorageService.InsertMessage("input-queue", newItem);
            // do something with emailAddress
        }
    }
}