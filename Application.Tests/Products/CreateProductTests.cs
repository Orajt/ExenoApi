using Application.Interfaces;
using Application.Products;
using Moq;
using NUnit.Framework;
using Domain;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using System.Collections.Generic;
using System;
namespace Application.Tests.Products
{
    [TestFixture]
    public class CreateProductTests
    {
         private Mock<IUnitOfWork>? _unitOfWork;
         private CreateProduct.Command? _command;
         private Product? _product;

        [SetUp]
        public void Setup()
        {
            _product=new Product(){Name="a", Description="a", Price=1};
            _unitOfWork=new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow=>uow.Products.Add(_product));
            _unitOfWork.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(true);
            _command=new CreateProduct.Command()
            {
                Product=_product
            };

        }
        [Test]
        public async Task CreateProduct_SaveChaangesFail_ReturnResultFailure()
        {
            _unitOfWork!.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(false);
            var handler = new CreateProduct.Handler(_unitOfWork.Object);
            var result = await handler.Handle(_command, CancellationToken.None);
            _unitOfWork.Verify(uow=>uow.SaveChangesAsync());
            Assert.That(result.Error.Length, Is.GreaterThan(0));
        }
        [Test]
        public async Task CreateProduct_EverythingGoesFine_ReturnResultSuccess()
        {
            var handler = new CreateProduct.Handler(_unitOfWork!.Object);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result.IsSuccess, Is.True);
        }


    }
}