using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Products
{
    public class SingleProduct
    {
        public class Query : IRequest<Result<Domain.Product>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Domain.Product>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Domain.Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.Products.GetSingleById(request.Id);

                if(product==null) return null;

                return Result<Domain.Product>.Success(product);
            }
        }
    }
}