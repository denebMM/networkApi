using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Controllers;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SocialNetwork.Tests
{
    public class PostControllerTests
    {


        private readonly PostController _crtl;
        private readonly SOCIALDBContext _dbcontext;

        //public readonly SOCIALDBContext _dbcontext;
        public PostControllerTests()
        {

            //_crtl = new UserController(new SOCIALDBContext());
            var options = new DbContextOptionsBuilder<SOCIALDBContext>()
             .UseInMemoryDatabase(databaseName: "TestDatabase")
             .Options;
            _dbcontext = new Models.SOCIALDBContext(options);
            _crtl = new PostController(_dbcontext);
        }

       
        #region List of Posts
        [Fact]
        public void GetPosts_ReturnsListOfPosts()
        {
            // Arrange
            var posts = new List<Post>
        {
            new Post { Id = 1, Content = "Post 1", UserId = 1 },
            new Post { Id = 2, Content = "Post 2", UserId = 2 }
            // Add more mock posts if needed
        };

            _dbcontext.AddRange(posts);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetPosts();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
            
        }
        #endregion

        #region GetPost

        [Fact]
        public void GetPost_Ok()
        {
            // Crear algunos usuarios en la base de datos en memoria
            Post post1 = new Post { Id = 3, Content = "Silla" };
            Post post2 = new Post { Id = 4, Content = "Mesa" };
            _dbcontext.Posts.Add(post1);
            _dbcontext.Posts.Add(post2);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetPost(3);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public void GetPost_ReturnsBadRequest()
        {
            // Act
            var result = _crtl.GetPost(4);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal("post not found", objectResult.Value);
        }
        #endregion


        #region   Create post

        [Fact]
        public void CreatePost_ReturnsSuccessMessage()
        {
            // Arrange
            var model = new Post { Id = 7, Content = "Sky" };

            // Act
            var result = _crtl.CreatePost(model);

            // Assert

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
            // Add more assertions if needed
        }

        [Fact]
        public void CreatePost_ReturnsBadRequest()
        {
            // Arrange
            Post model = null; // Usuario inválido

            // Act
            var result = _crtl.CreatePost(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            //Assert.Equal("Invalid request", objectResult.Value);
        }

        #endregion

        #region Edit Post
        [Fact]
        public void EditPost_ReturnsBadRequest()
        {
            // Arrange
            var model = new Post
            {
                Id = 2,
                Content = "mouse"
            };

            // Act
            var result = _crtl.EditPost(model);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("post not found", badRequestResult.Value);
        }
        [Fact]
        public void EditPost_ReturnsSuccessMessage()
        {
            // Arrange
            var post = new Post { Id = 6, Content = "split" };
            _dbcontext.Posts.Add(post);
            _dbcontext.SaveChanges();

            var model = new Post { Id = 6, Content = "#shameofyou" };

            // Act
            var result = _crtl.EditPost(model);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        }

        #endregion

        #region Like

        [Fact]
        public void LikePost_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
           
            // Act
            var result = _crtl.LikePost(3, 4);

            // Assert
           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
           
        }

        #endregion



        #region Unlike

        [Fact]
        public void UnlikePost_UserNotFound_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = _crtl.UnlikePost(3, 4);

            // Assert

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        }

        #endregion


        #region Get walls
        [Fact]
        public void GetWall_UserExistsWithPosts_ReturnsOk()
        {
            // Arrange
            
           User user = new User { Id = 8,FullName="Loyda"};
            _dbcontext.Users.Add(user);
            _dbcontext.SaveChanges();
            var post = new Post { Id = 6, Content = "Tv",UserId=8 };
            _dbcontext.Posts.Add(post);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetWall(8);

            // Assert
         
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
          
        }

        [Fact]
        public void GetWall_UserDoesNotExist_ReturnsBadRequest()
        {
            
            // Act
            var result = _crtl.GetWall(90);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            // Validate response content for user not found if needed
        }
        #endregion

    }
}
