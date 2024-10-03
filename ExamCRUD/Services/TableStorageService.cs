using Azure;
using Azure.Data.Tables;
using AzureTeacherStudentSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExamCRUD.Services
{
    public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;
        private readonly ICacheService _cacheService;

        public TableStorageService(TableServiceClient tableServiceClient, string tableName, ICacheService cacheService)
        {
            _tableClient = tableServiceClient.GetTableClient(tableName);
            _cacheService = cacheService;
            _tableClient.CreateIfNotExists();
        }
        private string CacheKey => $"{typeof(T).Name}-AllEntities";

        public async Task<T> GetEntityAsync(string rowKey)
        {
            var cachedData = await _cacheService.GetCacheData<List<string>>(CacheKey);
            if (cachedData != null && cachedData.Count > 0)
            {
                var entities = cachedData.Select(json => JsonSerializer.Deserialize<T>(json)).ToList();
                var entity = entities.FirstOrDefault(e => e.RowKey == rowKey);
                if (entity != null)
                {
                    return entity;
                }
            }

            var entityFromTable = await _tableClient.GetEntityAsync<T>(typeof(T).Name, rowKey);

            return entityFromTable;
        }

        public async Task AddEntityAsync(T entity)
        {
            await _tableClient.AddEntityAsync(entity);

            await UpdateCacheAfterModification();
        }

        public async Task UpdateEntityAsync(T entity)
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);

            await UpdateCacheAfterModification();
        }

        public async Task DeleteEntityAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync(typeof(T).Name, rowKey);

            await UpdateCacheAfterModification();
        }

        public async Task<List<T>> GetAllEntitiesAsync()
        {
            var cachedData = await _cacheService.GetCacheData<List<string>>(CacheKey);
            if (cachedData != null && cachedData.Count > 0)
            {
                var entities = cachedData.Select(json => JsonSerializer.Deserialize<T>(json)).ToList();
                return entities;
            }

            var entitiesFromTable = _tableClient.Query<T>(e => e.PartitionKey == typeof(T).Name).ToList();

            var serializedEntities = entitiesFromTable.Select(entity => JsonSerializer.Serialize(entity)).ToList();

            await _cacheService.AddCacheData(CacheKey, serializedEntities);

            return entitiesFromTable;
        }

        private async Task UpdateCacheAfterModification()
        {
            var entitiesFromTable = _tableClient.Query<T>(e => e.PartitionKey == typeof(T).Name).ToList();

            var serializedEntities = entitiesFromTable.Select(entity => JsonSerializer.Serialize(entity)).ToList();

            await _cacheService.AddCacheData(CacheKey, serializedEntities);
        }
    }
}
