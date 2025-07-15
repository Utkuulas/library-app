namespace LibraryApp.Models
{
    public class FileEntity
    {
        public int Id { get; set; }

        public string? FileName { get; set; }

        public string? FileType { get; set; }

        public byte[]? Data { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
