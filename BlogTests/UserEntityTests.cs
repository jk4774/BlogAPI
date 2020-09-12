using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogData.Entities;
using NUnit.Framework;

namespace BlogTests
{
    public class UserEntityTests
    {
        [TestCase("", "")]
        [TestCase(null, null)]
        public void UserEntity_EmailAndPasswordAreNullOrWhitespace_ShouldReturnFalse(string email, string password)
        {
            var user = new User { Email = email, Password = password };
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult> { };
            var result = Validator.TryValidateObject(user, validationContext, validationResults, true);

            Assert.That(result == false);
        }
    }
}
