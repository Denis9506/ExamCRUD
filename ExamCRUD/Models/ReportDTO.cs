namespace ExamCRUD.Models
{
    public class ReportDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public Author Author { get; set; }  
    }
}
