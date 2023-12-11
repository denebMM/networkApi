using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Controllers;
using SocialNetwork.Models;
using System.Collections;
using System.Collections.Generic;
using Xunit;



namespace SocialNetwork.Tests
{
    public class UserControllerTests
    {


        private readonly UserController _crtl;
        private readonly SOCIALDBContext _dbcontext;

        //public readonly SOCIALDBContext _dbcontext;
        public UserControllerTests()
        {

            //_crtl = new UserController(new SOCIALDBContext());
            var options = new DbContextOptionsBuilder<SOCIALDBContext>()
             .UseInMemoryDatabase(databaseName: "TestDatabase")
             .Options;
            _dbcontext = new SOCIALDBContext(options);
            _crtl = new UserController(_dbcontext);
        }

        #region GetUser

        [Fact]
        public void GetUser_Ok()
        {
            // Crear algunos usuarios en la base de datos en memoria
            User user1 = new User { Id = 1, FullName = "User1" };
            User user2 = new User { Id = 2, FullName = "User2" };
            _dbcontext.Users.Add(user1);
            _dbcontext.Users.Add(user2);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetUser(2);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public void GetUser_ReturnsBadRequest()
        {
            // Act
            var result = _crtl.GetUser(4);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal("user not found", objectResult.Value);
        }
        #endregion


        #region GetUsers
        [Fact]
        public void GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 3, FullName = "User3" },
                new User { Id = 4, FullName = "User4" }
            };
            _dbcontext.Users.AddRange(users);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetUsers();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        }

        //[Fact]
        //public void GetUsers_Count()
        //{
        //    // Arrange
        //    var users = new List<User>
        //    {
        //        new User { Id = 5, FullName = "User5" },
        //        new User { Id = 6, FullName = "User6" }
        //    };
        //    _dbcontext.Users.AddRange(users);
        //    _dbcontext.SaveChanges();
        //    // Act
        //    var actionResult = _crtl.GetUsers();
        //    //Assert
        //    //var objectResult = Assert.IsType<ObjectResult>(actionResult);  // Asegurar que la acción devuelva un ObjectResult
        //    var model = Assert.IsAssignableFrom<List<User>>(actionResult);  // Extraer y asegurar que el valor contenido en ObjectResult sea una lista de usuarios
        //    Assert.Equal(2, model.Count);
        //}

        #endregion

        #region Edit User
        [Fact]
        public void EditUser_ReturnsBadRequest()
        {
            // Arrange
            var model = new User
            {
                Id = 23,
                FullName = "John Doe"
            };

            // Act
            var result = _crtl.EditUser(model);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("user not found", badRequestResult.Value);
        }
        [Fact]
        public void EditUser_ReturnsSuccessMessage()
        {
            // Arrange
            var user = new User { Id = 6, FullName = "John Doe" };
            _dbcontext.Users.Add(user);
            _dbcontext.SaveChanges();

            var model = new User { Id = 6, FullName = "Jane Doe" };

            // Act
            var result = _crtl.EditUser(model);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        }

        #endregion


        #region   Create perfil user

        [Fact]
        public void CreateUser_ReturnsSuccessMessage()
        {
            // Arrange
            var model = new User { Id = 7, FullName = "Deep" };

            // Act
            var result = _crtl.CreateUser(model);

            // Assert

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
            // Add more assertions if needed
        }

        [Fact]
        public void CreateUser_ReturnsBadRequest()
        {
            // Arrange
            User model = null; // Usuario inválido

            // Act
            var result = _crtl.CreateUser(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            //Assert.Equal("Invalid request", objectResult.Value);
        }

        #endregion


        #region Create realitioship friend


        [Fact]
        public void AddFriend_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int friendId = 1; // Mismo userId y followerId

            // Act
            var result = _crtl.AddFriend(userId, friendId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            // Add more assertions if needed
        }

      
        #endregion

        #region Create realitioship followers


        [Fact]
        public void AddFollower_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int followerId = 1; // Mismo userId y followerId

            // Act
            var result = _crtl.AddFollower(userId, followerId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            // Add more assertions if needed
        }

        //[Fact]
        //public void AddfollowerToDatabase_SuccessfullyAdded_ReturnsTrue()
        //{
        //    // Arrange
        //    int userId = 1;
        //    int followerId = 2;

        //    // Act
        //    var success = _crtl.AddFollower(userId, followerId);

        //    // Assert
        //    Assert.True(success);
        //    
        //}
        #endregion


        #region Get user friend
        [Fact]
        public void GetUserFriend_ValidFriend_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            var mockUserFriend = new UserFriend { UserId = userId, FriendId = friendId };
            _dbcontext.UserFriends.Add(mockUserFriend); 

            // Act
            var result = _crtl.GetUserFriend(userId, friendId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
          
        }


        #endregion

        #region Get user follower
        [Fact]
        public void GetUserFollower_ValidFriend_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            int followerId = 2;
            var follower = new UserFollower { UserId = userId, FollowerId = followerId };
            _dbcontext.UserFollowers.Add(follower);

            // Act
            var result = _crtl.GetUserFollower(userId, followerId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        }


        #endregion

        #region Get Followers

        [Fact]
        public void GetFollowers_ReturnsListOfFollowers()
        {
            // Arrange
            var mockFollowers = new List<UserFollower>
            {
                new UserFollower { UserId = 1, FollowerId = 2 },
                new UserFollower { UserId = 1, FollowerId = 3 }
                
            };      

            _dbcontext.UserFollowers.AddRange(mockFollowers);
               _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetFollowers();

            // Assert
           
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
           
        }
        #endregion



        #region Get Friends

        [Fact]
        public void GetFriends_ReturnsListOfFollowers()
        {
            // Arrange
            var mockFollowers = new List<UserFriend>
            {
                new UserFriend { UserId = 1, FriendId = 2 },
                new UserFriend { UserId = 1, FriendId = 3 }
            };

            _dbcontext.UserFriends.AddRange(mockFollowers);
            _dbcontext.SaveChanges();

            // Act
            var result = _crtl.GetFriends();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        }
        #endregion

    }
}