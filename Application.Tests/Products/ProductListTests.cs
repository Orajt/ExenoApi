using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Products;
using Domain;
using Moq;
using NUnit.Framework;

namespace Application.Tests.Products
{
    public class ProductListTests
    {
        private Product? _product;
        private Mock<IUnitOfWork>? _unitOfWork;
        private List<Product>? _products;
        private ProductList.Query? _query;

        [SetUp]
        public void Setup()
        {
            _product = new Product() { Name = "a", Description = "a", Price = 1 };
            _products=new List<Product>(){
                _product,
                _product
            };
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow => uow.Products.GetAllProducts()).ReturnsAsync(_products);
        }
        [Test]
        public async Task ProductList_EverythingGoesWell_ReturnListProduct()
        {
            var handler = new ProductList.Handler(_unitOfWork!.Object);
            var result = await handler.Handle(_query, CancellationToken.None);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(2));
            Assert.That(result.Value[1],Is.EqualTo(_products![1]));
        }

    }
}