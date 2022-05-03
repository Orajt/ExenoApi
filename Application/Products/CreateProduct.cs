using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Products
{
    public class CreateProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Product Product{get;set;}
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if(String.IsNullOrWhiteSpace(request.Product.Name))
                    return Result<Unit>.Failure("Name cannot be empty or contains only whitespaces");    
                if(request.Product.Price<0)
                    return Result<Unit>.Failure("Price cannot be lower than 0");

                request.Product.CreationDate=DateTime.Now;
                
                _unitOfWork.Products.Add(request.Product);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}