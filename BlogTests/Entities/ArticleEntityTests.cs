using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogData.Entities;
using NUnit.Framework;

namespace BlogTests.Entities
{
    public class ArticleEntityTests
    {
        [TestCase("", "")]
        [TestCase(null, null)]
        public void ArticleEntity_TitleAndContentAreNullOrWhitespace_ShouldReturnFalse(string title, string content)
        {
            var article = new Article { Title = title, Content = content };
            var validationContext = new ValidationContext(article);
            var validationResults = new List<ValidationResult> { };
            var result = Validator.TryValidateObject(article, validationContext, validationResults, true);

            Assert.That(result == false);
        }
    }
}
