using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Products;
using Domain;
using Moq;
using NUnit.Framework;

namespace Application.Tests.Products
{
    public class SingleProductTests
    {
        private Mock<IUnitOfWork>? _unitOfWork;
        private SingleProduct.Query? _query;
        private Product? _product;

        [SetUp]
        public void Setup()
        {
            _product = new Product() { Name = "a", Description = "a", Price = 1 };
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow => uow.Products.GetSingleById(1)).ReturnsAsync(_product);
            _query = new SingleProduct.Query()
            {
                Id = 1
            };
        }
        [Test]
        public async Task SingleProduct_ProductDoesntExistsInDatabase_ReturnResultNull()
        {
            _unitOfWork!.Setup(uow => uow.Products.GetSingleById(1)).ReturnsAsync((Product)null!);
            var handler = new SingleProduct.Handler(_unitOfWork.Object);
            var result = await handler.Handle(_query, CancellationToken.None);
            Assert.That(result, Is.Null);
            _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
        [Test]
        public async Task SingleProduct_EverythingGoesWell_ReturnProduct()
        {
            var handler = new SingleProduct.Handler(_unitOfWork!.Object);
            var result = await handler.Handle(_query, CancellationToken.None);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(_product));
        }
    }
}