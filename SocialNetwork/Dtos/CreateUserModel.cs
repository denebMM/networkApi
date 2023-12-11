using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Dtos
{
    public class CreateUserModel
    {
        [Required]
        public string FullName { get; set; }
      
    }
}
