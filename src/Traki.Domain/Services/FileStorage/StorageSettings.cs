namespace Traki.Domain.Services.BlobStorage
{
    public class StorageSettings
    {
        public static readonly string SectionName = "BlobStorage";
        public string ConnectionString { get; set; }
    }
}
