using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dtos;
using SocialNetwork.Models;
using System.Security.Claims;

namespace SocialNetwork.Controllers
{
    public class PostController : Controller
    {
        public readonly SOCIALDBContext _dbcontext;

        public PostController(SOCIALDBContext _context)
        {

            _dbcontext = _context;
        }

        #region List of Posts
        [HttpGet]
        [Route("posts")]
        public ActionResult GetPosts()
        {
            List<Post> list = new List<Post>();
            try
            {
                list = _dbcontext.Posts.Include(p => p.User).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }
        #endregion

        #region Get un post by id
        [HttpGet("idPost")]
        public ActionResult GetPost(int id)
        {
            Post post = _dbcontext.Posts.Find(id);
            if (post == null)
            {
                return BadRequest("post not found");
            }
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = post });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = post });
            }
        }
        #endregion

        #region Create Post

        [HttpPost]
        [Route("posts")]
        public ActionResult CreatePost([FromBody] Post model)
        {

            try
            {
                if (model != null)
                {
                    _dbcontext.Posts.Add(model);
                    _dbcontext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { message = "The post has been created successfully" });
                }
                return BadRequest(new { error = "Invalid request" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = ex.Message });
            }
           
        }

        #endregion

        #region Edit Post
        [HttpPut]
        [Route("posts")]
        public ActionResult EditPost([FromBody] Post model)
        {
            Post post = _dbcontext.Posts.Find(model.Id);
            if (post == null)
            {
                return BadRequest("post not found");
            }
            try
            {
                post.Content= model.Content is null ? post.Content : model.Content;
                post.IsPublic = model.IsPublic;
                post.Likes = model.Likes is null ? post.Likes : model.Likes;
                _dbcontext.Posts.Update(post);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "post update" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }

        }

        #endregion

        #region  Like

        [HttpPost("{userId}/like/{postId}")]
        public ActionResult LikePost(int userId,int postId)
        {
            var user= _dbcontext.Users.Find(userId);
            var post = _dbcontext.Posts.Find(postId);
            if (user == null)
            {
                return BadRequest("There is no user with id: " + userId);
            }
            if (post == null)
            {
                return BadRequest("There isn't a post with id: " + postId);
            }
            if (!post.IsPublic)
            {
                return BadRequest("This post is private");
            }
            var like =  _dbcontext.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);
            if (like == null)
            {
               
                _dbcontext.Likes.Add(new Like { UserId = userId, PostId = postId });
                _dbcontext.SaveChanges();
                //Update likes in post
                post.Likes += 1;
                _dbcontext.Posts.Update(post);
                _dbcontext.SaveChanges();
            }

            return StatusCode(StatusCodes.Status200OK, new { message = user.FullName + " like " + post.Content  });
        }
        #endregion

        #region  Unlike
        [HttpPost("{userId}/unlike/{postId}")]
        public ActionResult UnlikePost(int userId, int postId)
        {
            var user = _dbcontext.Users.Find(userId);
            var post = _dbcontext.Posts.Find(postId);
            if (user == null)
            {
                return BadRequest("There is no user with id: " + userId);
            }
            if (post == null)
            {
                return BadRequest("There isn't a post with id: " + postId);
            }
            if (!post.IsPublic)
            {
                return BadRequest("This post is private");
            }

            var like = _dbcontext.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);

            if (like != null)
            {
                _dbcontext.Likes.Remove(like);
                _dbcontext.SaveChanges();
                //Update likes in post
                post.Likes -= post.Likes != 0 ? 1 : 0 ;
                _dbcontext.Posts.Update(post);
                _dbcontext.SaveChanges();
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "ok"});
        }
        #endregion


        #region   Get wall by userId

        [HttpGet("walls/{userId}")]
        public ActionResult GetWall(int userId)
        {
            var user = _dbcontext.Users.Find(userId);
            if (user == null)
            {
                return BadRequest("There is no user with id: " + userId);
            }

            var posts = _dbcontext.Posts
            .Where(p => p.UserId == userId && p.IsPublic)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

            if (posts == null)
            {
                return NotFound("No posts found.");
            }

            return StatusCode(StatusCodes.Status200OK, new { message = "A list of all the posts made by the user, their friends, and people they follow, sorted by timestamp descending.",responde=posts });
        }
        #endregion


    }
}
