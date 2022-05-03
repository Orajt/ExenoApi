using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Products;
using Domain;
using Moq;
using NUnit.Framework;

namespace Application.Tests.Products
{
    [TestFixture]
    public class DeleteProductTests
    {
         private Mock<IUnitOfWork>? _unitOfWork;
         private DeleteProduct.Command? _command;
         private Product? _product;

        [SetUp]
        public void Setup()
        {
            _product=new Product(){Name="a", Description="a", Price=1};
            _unitOfWork=new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow=>uow.Products.GetSingleById(1)).ReturnsAsync(_product);
            _unitOfWork.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(true);
            _command=new DeleteProduct.Command()
            {
                Id=1
            };
        }
        [Test]
        public async Task DeleteProduct_ProductDoesntExistsInDatabase_ReturnResultNull()
        {
            _unitOfWork?.Setup(uow=>uow.Products.GetSingleById(1)).ReturnsAsync((Product)null!);
            var handler = new DeleteProduct.Handler(_unitOfWork?.Object);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result, Is.Null);
            _unitOfWork?.Verify(uow=>uow.SaveChangesAsync(),Times.Never);
        }
        [Test]
        public async Task DeleteProduct_SaveChaangesFail_ReturnResultFailure()
        {
            _unitOfWork?.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(false);
            var handler = new DeleteProduct.Handler(_unitOfWork?.Object);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result.Error.Length, Is.GreaterThan(0));
        }
        [Test]
        public async Task DeleteProduct_EverythingGoesWell_ReturnResultSuccess()
        {
            var handler = new DeleteProduct.Handler(_unitOfWork?.Object);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result.IsSuccess, Is.True);
        }



    }
}