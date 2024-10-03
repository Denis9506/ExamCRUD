using Azure;
using Azure.Data.Tables;
using ExamCRUD.Models;
using System;

namespace ExamCRUD.Model
{
    public class Report : ITableEntity
    {
        public Report()
        {
            PartitionKey = nameof(Report);
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ETag = new();
        }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;

        public string AuthorRowKey { get; set; } = string.Empty;
        [NoFutureDate(ErrorMessage = "The published date cannot be in the future.")]
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
