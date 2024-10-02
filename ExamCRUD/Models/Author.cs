using Azure;
using Azure.Data.Tables;
using ExamCRUD.Model;
using System.Data;

namespace ExamCRUD.Models
{
    public class Author : ITableEntity
    {
        public Author()
        {
            PartitionKey = nameof(Author);
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ETag = new();
        }

        public string NickName { get; set; } = string.Empty;
        public Role Role { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
public enum Role
{
    Admin,
    Editor,
    Viewer
}