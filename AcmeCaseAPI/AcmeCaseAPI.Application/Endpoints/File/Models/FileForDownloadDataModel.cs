namespace AcmeCaseAPI.Application.Endpoints.File.Models
{
    public class FileForDownloadDataModel
    {
        public string FileDisplayName { get; set; }
        public string FileTypeExtension { get; set; }
        public byte[] FileData { get; set; }
    }
}
