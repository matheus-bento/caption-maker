namespace CaptionMaker.Files
{
    public class CaptionMakerFilesOptions
    {
        [ConfigurationKeyName("BASE_FILE_PATH")]
        public string BaseFilePath { get; set; }

        [ConfigurationKeyName("API_KEY")]
        public string ApiKey { get; set; }
    }
}
