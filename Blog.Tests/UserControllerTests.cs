using Blog.API.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void GET_GetById_IncorrectUserId_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.GetById(1);
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void GET_GetById_CorrectUserId_User()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "qwer", Password = "Username121!", Email = "test@test.com" });
            var User = userController.GetById(1);
            Assert.IsType<ActionResult<User>>(User);
        }

        [Fact]
        public void POST_Login_UserIsNull_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Login(null);
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserNameIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Login(new User { Name = " ", Password = "Username121!" });
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserPasswordIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Login(new User { Name = "Username12", Password = " " });
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_BothUserNameAndPasswordAreWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Login(new User { Name = "", Password = " " });
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithWrongName_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Login(new User { Name = "INCORRENTUSERNAME154543", Password = "Username121!" });
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithCorrectNameAndWrongPassword_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Username12", Password = "Username121@", Email = "email@email.com" });
            var User = userController.Login(new User { Name = "Username12", Password = "Username121!" });
            Assert.IsType<NotFoundResult>(User.Result);
        }

        [Fact]
        public void POST_Login_UserWithCorrectBothNameAndPassword_NoContent()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 998, Name = "Username12", Password = "Username121!", Email = "email77@email77.com" });
            var User = userController.Login(new User { Name = "Username12", Password = "Username121!" });
            Assert.IsType<ActionResult<User>>(User);
        }

        [Fact]
        public void POST_Register_NullUser_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(null);
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_NameIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = " ", Password = "Username121!!!@#", Email = "test!@4art.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_PasswordIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = "Username12", Password = " ", Email = "test!@#4art.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_EmailIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = "Username12", Password = "Username12!@1", Email = "" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserNamePasswordEmailAreWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = " ", Password = " ", Email = "" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_CorrectUser_NoContent()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 2, Name = "Username12", Password = "Username12!", Email = "test@test7.com" });
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void POST_Register_UserWithThisEmailExist_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Username12", Password = "Username121!", Email = "test@tesT00.com" });
            var User = userController.Register(new User { Id = 1, Name = "Username123", Password = "Username121!", Email = "test@tesT00.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_UserWithThisNameExist_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "UserName5", Password = "UserName51!", Email = "test@test77.com" });
            var User = userController.Register(new User { Id = 1, Name = "UserName5", Password = "UserName51!", Email = "test@test78.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        #region NewTests
        [Fact]
        public void POST_Register_LengthOfNameIsLessThan4_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = "asd", Password = "Qwerty1!", Email = "TEST@TEST.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_LengthOfPasswordIsLessThen8_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = "Qwerty1234", Password = "Qwert", Email = "TEST@TEST.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void POST_Register_LengthOfEmailIsLessThen8_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Register(new User { Id = 1, Name = "Qwerty1234", Password = "Qwerty1!", Email = "T@T.com" });
            Assert.IsType<NotFoundResult>(User);
        }
        #endregion

        [Fact]
        public void PUT_UpdateUser_UpdatedUserIsNull_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, null);
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdatedUserNameIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "", Password = "Username1!!!", Email = "asdf@aer.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdatedUserPasswordIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "Username123", Password = " ", Email = "asdf@aer" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdatedUserEmailIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "Username1234", Password = "Username1!!!23", Email = " " });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_EveryValueInUpdatedUserIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, new User { Id = 1, Name = " ", Password = " ", Email = " " });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdateExistingUser_NoContent()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyuiopp", Password = "Qwertyuiopp1!", Email = "test@test3333.com" });
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "Qwertyuiopp2", Password = "Qwertyuiopp2@", Email = "test@test33.com" });
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdateNotExistingUser_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "qwertyuio", Password = "Qwerty!1!!!", Email = "test@test.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdateWithTheSameName_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "name", Password = "pass", Email = "test@test.com" });
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "name", Password = "qwer", Email = "test21234@test.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdateWithTheSameEmail_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "name1", Password = "pass1", Email = "qwer@qwer.com" });
            var User = userController.UpdateUser(1, new User { Id = 1, Name = "name1", Password = "pass2", Email = "qw3r@QWER.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdateUser_UpdateWithTheSameBothNameAndEmail_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "name4", Password = "qwe3!", Email = "EMAIL@EMAIL.com" });
            var User = userController.UpdateUser(1, new User { Id = 55555, Name = "name4", Password = "qwer3!", Email = "email@EMAIL.com" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdatePassword_UserDoesNotExist_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.UpdatePassword(1, new Password { Old = "Qwerty1!", New = "Qwerty2!" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdatePassword_OldPasswordIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyu1awer", Password = "Qwertyu1awer1!", Email = "EMAIL@EMAIL.com" });
            var User = userController.UpdatePassword(1, new Password { Old = "", New = "Qwertyu1awer22" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdatePassword_NewPasswordIsWhiteSpace_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyuiop", Password = "Qwerty1!1!", Email = "EMAIL@EMAIL.com" });
            var User = userController.UpdatePassword(1, new Password { Old = "a", New = " " });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdatePassword_OldPasswordIsWrong_NotFound()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyuiopp!1", Password = "Qwertyuiopp!1!", Email = "EMAIL@EMAIL.com" });
            var User = userController.UpdatePassword(1, new Password { Old = "Qwertyuiopp!1!f", New = "newPass1234!" });
            Assert.IsType<NotFoundResult>(User);
        }

        [Fact]
        public void PUT_UpdatePassword_OldPasswordIsOkay_NoContent()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyuiopp", Password = "Qwertyuiopp!1", Email = "EMAIL@EMAIL.com" });
            var User = userController.UpdatePassword(1, new Password { Old = "Qwertyuiopp!1", New = "r1Qwertyuiopp!" });
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingUser_NoContent()
        {
            var userController = Utils.GetUserController();
            userController.Register(new User { Id = 1, Name = "Qwertyuiopp", Password = "Qwertyuiopp1!", Email = "test@test.com" });
            var User = userController.Delete(1);
            Assert.IsType<NoContentResult>(User);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingUser_NotFound()
        {
            var userController = Utils.GetUserController();
            var User = userController.Delete(1);
            Assert.IsType<NotFoundResult>(User);
        }
    }
}
