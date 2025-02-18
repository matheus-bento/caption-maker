namespace CaptionMaker
{
    class CaptionMakerOptions
    {
        [ConfigurationKeyName("DB_CONNECTION_STRING")]
        public string DbConnectionString { get; set; } = String.Empty;
    }
}
