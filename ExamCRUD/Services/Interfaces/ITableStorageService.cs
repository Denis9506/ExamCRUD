using Azure.Data.Tables;

namespace ExamCRUD.Services
{
    public interface ITableStorageService<T> where T : class, ITableEntity, new()
    {
        Task<T> GetEntityAsync(string rowKey);
        Task AddEntityAsync(T entity);
        Task UpdateEntityAsync(T entity);
        Task DeleteEntityAsync(string rowKey);
        Task<List<T>> GetAllEntitiesAsync();
    }
}
