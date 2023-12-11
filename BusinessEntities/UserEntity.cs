using System.ComponentModel.DataAnnotations;

namespace BusinessEntities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "You must enter a name")]
        public string FullName { get; set; }
        public virtual ICollection<PostEntity> Posts { get; set; }
        public ICollection<UserEntity> Friends { get; set; }
        public ICollection<UserEntity> Followers { get; set; }

    }
}