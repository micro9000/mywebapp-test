using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using MyWebAppTest.Models;

namespace MyWebAppTest.Services;

public class TableStorageService : ITableStorageService
{
    private readonly string _storageConnectionString;
    private readonly string _tableName;

    public TableStorageService(IConfiguration configuration)
    {
        _storageConnectionString = configuration["StorageConnectionString"];
        _tableName = configuration["storageTableName"];
    }

    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_storageConnectionString);
        var tableClient = serviceClient.GetTableClient(_tableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }

    public async Task<GroceryItemEntity> GetEntityAsync(string category, string id)
    {
        var tableClient = await GetTableClient();
        return await tableClient.GetEntityAsync<GroceryItemEntity>(category, id);
    }
    public async Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity)
    {
        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(entity);
        return entity;
    }
    public async Task DeleteEntityAsync(string category, string id)
    {
        var tableClient = await GetTableClient();
        await tableClient.DeleteEntityAsync(category, id);
    }
}