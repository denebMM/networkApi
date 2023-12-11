using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dtos;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
 
    public class UserController : Controller
    {

        public readonly SOCIALDBContext _dbcontext;
        public UserController(SOCIALDBContext _context)
        {

            _dbcontext = _context;
        }

        #region Get users
        [HttpGet]
        [Route("users")]
        public ActionResult GetUsers()
        {
            List<User> list = new List<User>();
            try
            {
                list = _dbcontext.Users.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }
        #endregion

        #region Get user by id
        [HttpGet("id")]
        public ActionResult GetUser(int id)
        {
               User user = _dbcontext.Users.Find(id);
                if (user == null)
                { 
                    return BadRequest("user not found"); 
                }
                try
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = user });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = user });
                }
        }
        #endregion

        #region Edit perfiles de usuario

        [HttpPut]
        [Route("users")]
        public ActionResult EditUser([FromBody] User model)
        {
            User user = _dbcontext.Users.Find(model.Id);
            if (user == null)
            {
                return BadRequest("user not found");
            }
            try
            {
                user.Posts = model.Posts is null ? user.Posts : model.Posts;
                user.Likes = model.Likes is null ? user.Likes : model.Likes;
                user.UserFollowers = model.UserFollowers is null ? user.UserFollowers : model.UserFollowers;
                user.UserFriends = model.UserFriends is null ? user.UserFriends : model.UserFriends;
                user.FullName= model.FullName is null ? user.FullName : model.FullName;

                _dbcontext.Users.Update(user);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "user update" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        #endregion

        #region  Create perfil user

        [HttpPost]
        [Route("users")]
        public ActionResult CreateUser([FromBody] User model)
        {
           
            try
            {
                if (model != null)
                {
                    _dbcontext.Users.Add(model);
                    _dbcontext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { message = "The user has been created successfully" });
                }
                return BadRequest(new { error = "Invalid request" });
            }
            catch (Exception ex)
            {

                 return StatusCode(500, new { error = ex.Message });
            }

        }

        #endregion

        #region Create a relationship friends
        [HttpPost("users/{userId}/friends/{friendId}")]
        public ActionResult AddFriend(int userId, int friendId)
        {
            // Replace this with actual code that adds a friend
            var success = AddFriendToDatabase(userId, friendId);

            if (success)
            {
                return Ok(new { status = "success", message = "Successfully added the friendship relation" });
            }
            else
            {
                return BadRequest(new { status = "fail", message = "The request was invalid" });
            }
        }

        private bool AddFriendToDatabase(int userId, int friendId)
        {
            
            var user1 = _dbcontext.Users.Find(userId);
            var user2 = _dbcontext.Users.Find(friendId);

            if (userId == friendId || user1 == null || user1 == null || _dbcontext.UserFriends.Any(x => (x.UserId == userId && x.FriendId == friendId) || (x.UserId == friendId && x.FriendId == userId)))
            {
                return false;
            }
            else
            {
                _dbcontext.UserFriends.Add(new UserFriend { UserId = userId, FriendId = friendId });
                 _dbcontext.SaveChanges();
                return true;
            }
        }
        #endregion

        #region Create a relationship followers
        [HttpPost("users/{userId}/followers/{followerId}")]
        public ActionResult AddFollower(int userId, int followerId)
        {
            // Replace this with actual code that adds a friend
            var success = AddfollowerToDatabase(userId, followerId);

            if (success)
            {
                return Ok(new { status = "success", message = "Successfully added the friendship relation" });
            }
            else
            {
                return BadRequest(new { status = "fail", message = "The request was invalid" });
            }
        }

        private bool AddfollowerToDatabase(int userId, int followerId)
        {

            var user1 = _dbcontext.Users.Find(userId);
            var user2 = _dbcontext.Users.Find(followerId);

            if (userId == followerId || user1 == null || user1 == null || _dbcontext.UserFollowers.Any(x => (x.UserId == userId && x.FollowerId == followerId) || (x.UserId == followerId && x.FollowerId == userId)))
            {
                return false;
            }
            else
            {
                _dbcontext.UserFollowers.Add(new UserFollower { UserId = userId, FollowerId = followerId });
                _dbcontext.SaveChanges();
                return true;
            }
        }
        #endregion


        #region Get user friend

        [HttpGet("{id}/friends/{friendId}")]
        public  ActionResult  GetUserFriend(int id, int friendId)
        {
            var userFriend = _dbcontext.UserFriends.Find(id, friendId);
            if (userFriend == null)
            {
                return NotFound("The request was invalid");
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "Successfully added the friendship relation" });
            }
        #endregion

        #region Get user follower
        [HttpGet("{id}/followers/{followerId}")]
        public  ActionResult GetUserFollower(int id, int followerId)
        {
            var userFollower = _dbcontext.UserFollowers.Find(id, followerId);
            if (userFollower == null)
            {
                return NotFound("The request was invalid");
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "Successfully added the follower relation" });
        }
        #endregion

        #region List friends
        [HttpGet]
        [Route("friends")]
        public ActionResult GetFriends()
        {
            List<UserFriend> list = new List<UserFriend>();
            try
            {
                list = _dbcontext.UserFriends.ToList();
               
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }
        #endregion

        #region List followers
        [HttpGet]
        [Route("followers")]
        public ActionResult GetFollowers()
        {
            List<UserFollower> list = new List<UserFollower>();
            try
            {
                list = _dbcontext.UserFollowers.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }
        #endregion
    }
}
