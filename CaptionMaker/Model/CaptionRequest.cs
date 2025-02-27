namespace CaptionMaker.Model
{
    public class CaptionRequest
    {
        public IFormFile Image { get; set; }
        public string Caption { get; set; }
    }
}
