using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Products;
using AutoMapper;
using Domain;
using Moq;
using NUnit.Framework;

namespace Application.Tests.Products
{
    [TestFixture]
    public class EditProductTests
    {
         private Mock<IUnitOfWork>? _unitOfWork;
         private EditProduct.Command? _command;
         private Product? _newProduct;
         private Product? _productDB;
         private IMapper? _mapper;
         private DateTime _date;

        [SetUp]
        public void Setup()
        {
            _date = DateTime.Now;
            _newProduct=new Product(){Id=1, Name="b", Description="b", Price=2, CreationDate=_date};
            _productDB=new Product(){Id=1, Name="a", Description="a", Price=1, CreationDate=_date.AddDays(-2)};
            _unitOfWork=new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow=>uow.Products.GetSingleById(1)).ReturnsAsync(_productDB);
            _unitOfWork.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(true);
            _command=new EditProduct.Command()
            {
                Product=_newProduct
            };
           var mapperConfig = new MapperConfiguration(c=>{
               c.AddProfile<MappingProfiles>();
           });
           _mapper=mapperConfig.CreateMapper();

        }
        [Test]
        public async Task EditProduct_newProductDoesntExistsInDatabase_ReturnResultNull()
        {
            _unitOfWork!.Setup(uow=>uow.Products.GetSingleById(1)).ReturnsAsync((Product)null!);
            var handler = new EditProduct.Handler(_unitOfWork.Object, _mapper);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result, Is.Null);
            _unitOfWork.Verify(uow=>uow.SaveChangesAsync(),Times.Never);
        }
        [Test]
        public async Task EditProduct_TestPropertyChaanges_CreationDateStaysSameOtherPropsNewer()
        {
            var handler = new EditProduct.Handler(_unitOfWork!.Object, _mapper);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(_productDB!.Name, Is.EqualTo(_newProduct!.Name));
            Assert.That(_productDB.CreationDate, Is.EqualTo(_date.AddDays(-2)));
            Assert.That(result.IsSuccess, Is.True);
        }
        
        [Test]
        public async Task EditProduct_SaveChaangesFail_ReturnResultFailure()
        {
            _unitOfWork!.Setup(uow=>uow.SaveChangesAsync()).ReturnsAsync(false);
            var handler = new EditProduct.Handler(_unitOfWork.Object, _mapper);
            var result = await handler.Handle(_command, CancellationToken.None);
            _unitOfWork.Verify(uow=>uow.SaveChangesAsync());
            Assert.That(result.Error.Length, Is.GreaterThan(0));
        }
        [Test]
        public async Task EditProduct_EverythingGoesFine_ReturnResultSuccess()
        {
            var handler = new EditProduct.Handler(_unitOfWork!.Object, _mapper);
            var result = await handler.Handle(_command, CancellationToken.None);
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}