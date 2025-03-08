namespace CaptionMaker.Model
{
    public class SaveCaptionRequest
    {
        public IFormFile Image { get; set; }
        public string Caption { get; set; }
    }
}
