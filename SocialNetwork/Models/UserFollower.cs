using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public partial class UserFollower
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
