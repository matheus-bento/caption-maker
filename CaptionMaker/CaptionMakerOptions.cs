namespace CaptionMaker
{
    public class CaptionMakerOptions
    {
        [ConfigurationKeyName("DB_CONNECTION_STRING")]
        public string DbConnectionString { get; set; }

        [ConfigurationKeyName("JWT_SECRET")]
        public string JwtSecret { get; set; }

        [ConfigurationKeyName("IMAGE_STORAGE_URL")]
        public string ImageStorageUrl { get; set; }
    }
}
