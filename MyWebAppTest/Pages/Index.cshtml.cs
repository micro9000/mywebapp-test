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

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public IndexModel(ILogger<IndexModel> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            this.cosmosDbService = cosmosDbService;
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Log from index page");
            Items = await cosmosDbService.GetItemsAsync("SELECT * FROM c");
            //asdf
        }
    }
}