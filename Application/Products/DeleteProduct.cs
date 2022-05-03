using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Products
{
    public class DeleteProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
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
                var product = await _unitOfWork.Products.GetSingleById(request.Id);

                if(product==null) return null;

                _unitOfWork.Products.Remove(product);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to delete product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}