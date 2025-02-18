using System.ComponentModel.DataAnnotations;

namespace CaptionMaker.Data.Model
{
    public class Caption : BaseEntity
    {
        [Required]
        public string Filepath { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
