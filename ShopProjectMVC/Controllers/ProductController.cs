using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Products()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int productId)
        {
            try
            {
                var order = await _productService.BuyProduct(1, productId);

                return RedirectToAction("Orders","Order");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
