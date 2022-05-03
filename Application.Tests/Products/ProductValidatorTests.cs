using Application.Products;
using Domain;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.Tests.Products
{
    [TestFixture]
    public class ProductValidatorTests
    {
        private ProductValidator? validator;
        [SetUp]
        public void Setup()
        {
            validator = new ProductValidator();
        }
        [Test]
        [TestCase("", TestName ="ProductValidator_NameIsEmpty_ReturnError")]
        [TestCase("  ", TestName ="ProductValidator_NameContainsOnlyWhitespaces_ReturnError")]
        [TestCase(null, TestName ="ProductValidator_NameIsNull_ReturnError")]
        public void ProductValidator_ValidateName_ReturnsError(string name)
        {
            var model = new Product { Name = name, Price=1 };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(product => product.Name);
            result.ShouldNotHaveValidationErrorFor(product => product.Price);
        }
        [Test]
        public void ProductValidator_ValidatePrice_ReturnsError()
        {
            var model = new Product { Name = "a", Price=-1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(product => product.Name);
            result.ShouldHaveValidationErrorFor(product => product.Price);
        }
        [Test]
        public void ProductValidator_ProductWithGoodProperties_ValidationSuccess()
        {
            var model = new Product { Name = "a", Price=1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}