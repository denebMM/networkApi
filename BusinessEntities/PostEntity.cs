using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessEntities
{
    public class PostEntity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "You must enter a text")]
        public string Text { get; set; }
        public bool IsPublic { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }
        public int Likes { get; set; }
    }
}