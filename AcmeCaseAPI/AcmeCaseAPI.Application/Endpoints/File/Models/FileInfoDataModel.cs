namespace AcmeCaseAPI.Application.Endpoints.File.Models
{
    public class FileInfoDataModel
    {
        public int FileID { get; set; }
        public string FileDisplayName { get; set; }
        public string FileTypeExtension { get; set; }
        public string FileComment { get; set; }
    }
}
