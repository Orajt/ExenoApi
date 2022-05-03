using Domain;
using FluentValidation;

namespace Application.Products
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product=>product.Name).Must(p=>!string.IsNullOrWhiteSpace(p)).WithMessage("Name cannot be empty or contains only whitespaces");
            RuleFor(product=>product.Price).GreaterThanOrEqualTo(0).WithMessage("Price cannot be lower than 0");
        }
    }
}