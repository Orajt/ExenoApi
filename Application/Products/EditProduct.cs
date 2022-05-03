using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Products
{
    public class EditProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Product Product { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.Products.GetSingleById(request.Product.Id);

                if (product == null) return null;

                if (String.IsNullOrWhiteSpace(request.Product.Name))
                    return Result<Unit>.Failure("Name cannot be empty or contains only whitespaces");
                if (request.Product.Price < 0)
                    return Result<Unit>.Failure("Price cannot be lower than 0");

                request.Product.CreationDate=product.CreationDate;
                _mapper.Map(request.Product, product);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to edit product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}