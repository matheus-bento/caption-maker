using System.ComponentModel.DataAnnotations;

namespace CaptionMaker.Data.Model
{
    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
    }
}
