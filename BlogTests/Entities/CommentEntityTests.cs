using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogData.Entities;
using NUnit.Framework;

namespace BlogTests.Entities
{
    public class CommentEntityTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void CommentEntity_ContentIsNullOrWhitespace_ShouldReturnFalse(string content)
        {
            var comment = new Comment { Content = content };
            var validationContext = new ValidationContext(comment);
            var validationResults = new List<ValidationResult> { };
            var result = Validator.TryValidateObject(comment, validationContext, validationResults, true);

            Assert.That(result == false);
        }
    }
}
