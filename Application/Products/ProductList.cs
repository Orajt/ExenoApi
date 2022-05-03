using System.Collections.Generic;
using Application.Core;
using MediatR;
using Domain;
using Persistence;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Products
{
    public class ProductList
    {
        public class Query : IRequest<Result<List<Domain.Product>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<Domain.Product>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<Domain.Product>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _unitOfWork.Products.GetAllProducts();

                return Result<List<Domain.Product>>.Success(products);
            }
        }
    }
}