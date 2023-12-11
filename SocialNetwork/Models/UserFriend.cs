using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public partial class UserFriend
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
