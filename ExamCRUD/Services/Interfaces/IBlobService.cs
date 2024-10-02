namespace ExamCRUD.Services
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream);
        Task DeleteFileAsync(string containerName, string fileName);
        Task<bool> FileExistsAsync(string containerName, string fileName);
        Task<Stream> DownloadFileAsync(string containerName, string fileName);
    }
}
