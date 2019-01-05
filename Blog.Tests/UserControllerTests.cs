using Blog.API.Controllers;
using Blog.API.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void GET_GetById_IncorrectUserId_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.GetById(7777);

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void GET_GetById_CorrectUserId_User()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 1, Name = "name1", Password = "pass1", Email = "test@test.com" });
            var User = UserController.GetById(1);

            // Assert
            Assert.IsType<ActionResult<User>>(User);
        }

        [Fact]
        public void POST_Login_UserIsNull_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Login(null);

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserNameIsWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Login(new User { Name = " ", Password = "pass" });

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserPasswordIsWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Login(new User { Name = "asdf", Password = " " });

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_BothUserNameAndPasswordAreWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Login(new User { Name = "", Password = " " });

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithWrongName_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Login(new User { Name = "INCORRENTUSERNAME154543", Password = "pass1" });

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithCorrectNameAndWrongPassword_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 997, Name = "Tom", Password = "pass1", Email = "email@email.com" });
            var User = UserController.Login(new User { Name = "Tom", Password = "pass2" });

            // Assert
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithCorrectBothNameAndPassword_NoContent()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 998, Name = "Thomas", Password = "pass1", Email = "email77@email77.com" });
            var User = UserController.Login(new User { Name = "Thomas", Password = "pass1" });

            // Assert
            Assert.IsType<ActionResult<User>>(User);
        }

        [Fact]
        public void POST_Register_NullUser_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(null);

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserNameIsWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(new User { Id = 1, Name = " ", Password = "PASS!@#", Email = "test!@#4art.com" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserPasswordIsWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(new User { Id = 1, Name = "awer", Password = " ", Email = "test!@#4art.com" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserEmailIsWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(new User { Id = 1, Name = "aefaw", Password = "PASS!@#", Email = "" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserNamePasswordEmailAreWhiteSpace_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(new User { Id = 1, Name = " ", Password = " ", Email = "" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_CorrectUser_NoContent()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Register(new User { Id = 2, Name = "name000", Password = "pass1", Email = "test@test7.com" });

            // Assert
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void POST_Register_UserWithThisEmailExist_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 6, Name = "user6", Password = "pass1", Email = "test@tesT00.com" });
            var User = UserController.Register(new User { Id = 7, Name = "user7", Password = "pass1", Email = "test@test00.com" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserWithThisNameExist_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 8, Name = "user8", Password = "pass1", Email = "test@test77.com" });
            var User = UserController.Register(new User { Id = 9, Name = "useR8", Password = "pass1", Email = "test@test78.com" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdatedUserIsNull_NotFound()
        {
            // Arrange 
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(1, null);

            // Assert 
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdatedUserNameIsWhiteSpace_NotFound()
        {
            // Arrange 
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(1, new User { Id = 1, Name = "", Password = "pass", Email = "asdf@aer" });

            // Assert 
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdatedUserPasswordIsWhiteSpace_NotFound()
        {
            // Arrange 
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(1, new User { Id = 1, Name = "asdew", Password = " ", Email = "asdf@aer" });

            // Assert 
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdatedUserEmailIsWhiteSpace_NotFound()
        {
            // Arrange 
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(1, new User { Id = 1, Name = "zxcvasdf", Password = "pass", Email = " " });

            // Assert 
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_EveryValueInUpdatedUserIsWhiteSpace_NotFound()
        {
            // Arrange 
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(1, new User { Id = 1, Name = " ", Password = " ", Email = " " });

            // Assert 
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdateExistingUser_NoContent()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.Register(new User { Id = 3, Name = "name554", Password = "pass1", Email = "test@test3333.com" });
            var User = UserController.Update(3, new User { Id = 3, Name = "name33", Password = "pass2", Email = "test@test33.com" });

            // Assert
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void PUT_Update_UpdateNotExistingUser_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Update(77, new User { Id = 77, Name = "name", Password = "pass", Email = "test@test.com" });

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingUser_NoContent()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            UserController.DeleteUsers();
            UserController.Register(new User { Id = 4, Name = "name5", Password = "pass5", Email = "test@test.com" });
            var User = UserController.Delete(4);

            // Assert
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingUser_NotFound()
        {
            // Arrange
            var UserController = Utils.GetUserController();

            // Act
            var User = UserController.Delete(5);

            // Assert
            Assert.IsType<NotFoundResult>(User);
        }
    }
}
