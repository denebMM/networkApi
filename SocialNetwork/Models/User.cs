using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public partial class User
    {
        public User()
        {
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
            UserFollowers = new HashSet<UserFollower>();
            UserFriends = new HashSet<UserFriend>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserFollower> UserFollowers { get; set; }
        public virtual ICollection<UserFriend> UserFriends { get; set; }
    }
}
