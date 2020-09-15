using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogData.ViewModels;
using NUnit.Framework;

namespace BlogTests.ViewModels
{
    public class PasswordViewModelTests
    {
        [TestCase("", "")]
        [TestCase(null, null)]
        public void PasswordViewModel_OldAndNewAreNullOrWhitespace_ShouldReturnFalse(string oldPassword, string newPassword)
        {
            var passwordViewModel = new PasswordViewModel { Old = oldPassword, New = newPassword };
            var validationContext = new ValidationContext(passwordViewModel);
            var validationResults = new List<ValidationResult> { };
            var result = Validator.TryValidateObject(passwordViewModel, validationContext, validationResults, true);

            Assert.That(result == false);
        } 

        [Test]
        public void PasswordViewModel_OldAndNewHaveShorterLengthThan8_ShouldReturnFalse()
        {
            var passwordViewModel = new PasswordViewModel { Old = "TestPas", New = "TestP@s" };
            var validationContext = new ValidationContext(passwordViewModel);
            var validationResults = new List<ValidationResult> { };
            var result = Validator.TryValidateObject(passwordViewModel, validationContext, validationResults, true);

            Assert.That(result == false);
        }
    }
}