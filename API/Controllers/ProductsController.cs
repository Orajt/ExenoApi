using System.Threading.Tasks;
using Application.Products;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ProductsList()
        {
            return HandleResult(await Mediator.Send(new ProductList.Query()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> SingleProduct(int id)
        {
            return HandleResult(await Mediator.Send(new SingleProduct.Query(){Id=id}));
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            return HandleResult(await Mediator.Send(new CreateProduct.Command(){Product=product}));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, Product product)
        {
            product.Id=id;
            return HandleResult(await Mediator.Send(new EditProduct.Command(){Product=product}));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteProduct.Command(){Id=id}));
        }
    }
}