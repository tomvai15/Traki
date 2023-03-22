namespace Traki.Api.Services.BlobStorage
{
    public class BlobStorageSettings
    {
        public static readonly string SectionName = "BlobStorage";
        public string ConnectionString { get; set; }
        public string BaseUri { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }
}
