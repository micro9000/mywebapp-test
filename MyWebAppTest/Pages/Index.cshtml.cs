using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
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
        private readonly IBlobStorageService blobStorageService;
        private readonly Settings settings;

        public Settings MySettings { get { return settings; } }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public IndexModel(ILogger<IndexModel> logger, 
                            ICosmosDbService cosmosDbService, 
                            IQueueStorageService<Item> queueStorageService,
                            IBlobStorageService blobStorageService,
                            IOptionsSnapshot<Settings> options)
        {
            _logger = logger;
            this.cosmosDbService = cosmosDbService;
            this.queueStorageService = queueStorageService;
            this.blobStorageService = blobStorageService;
            settings = options.Value;
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Log from index page");
            Items = await cosmosDbService.GetItemsAsync("SELECT * FROM c");
            //asdf
        }

        public void OnPostSubmitQueueMessage()
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

        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task<IActionResult> OnPostUploadFile()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await blobStorageService.UploadBlob(Upload.OpenReadStream(), Upload.FileName);

            return Page();
        }
    }
}