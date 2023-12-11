using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public partial class Post
    {
        public Post()
        {
            LikesNavigation = new HashSet<Like>();
        }

        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public bool IsPublic { get; set; }
        public int? Likes { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Like> LikesNavigation { get; set; }
    }
}
