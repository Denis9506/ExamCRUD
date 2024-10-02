using Azure;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ExamCRUD.Services
{
    public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public TableStorageService(TableServiceClient tableServiceClient, string tableName)
        {
            _tableClient = tableServiceClient.GetTableClient(tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<T> GetEntityAsync(string rowKey)
        {
            return await _tableClient.GetEntityAsync<T>(typeof(T).Name, rowKey);
        }

        public async Task AddEntityAsync(T entity)
        {
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task UpdateEntityAsync(T entity)
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteEntityAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync(typeof(T).Name, rowKey);
        }

        public async Task<List<T>> GetAllEntitiesAsync()
        {
            var entities = _tableClient.Query<T>(e => e.PartitionKey == typeof(T).Name).ToList();
            return await Task.FromResult(entities);
        }
    }
}
