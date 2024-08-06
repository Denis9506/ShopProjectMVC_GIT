using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Filters;

namespace ShopProjectMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IProductService productService, IWebHostEnvironment webHost)
        {
            _productService = productService;
            _environment = webHost;
        }


        [AuthorizeFilter]
        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetAll().AsQueryable().ToListAsync();
            return View(products);
        }

        [AuthorizeFilter]
        public IActionResult Create()
        {
            ViewBag.Categories = _productService.GetAllCategories().ToList();
            return View();
        }

        [AuthorizeFilter]
        [HttpPost]
        public async Task<IActionResult> Buy(int productId)
        {
            try
            {
                int id = HttpContext.Session.GetInt32("userId").Value;
                var order = await _productService.BuyProduct(id, productId);

                return RedirectToAction("Orders", "Order");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


        [AuthorizeFilter]
        [HttpPost]
        public async Task<IActionResult> Create(Product product, int category, IFormFile file)
        {
            string hash = Guid.NewGuid().ToString();
            string name = Path.GetFileNameWithoutExtension(file.FileName) + hash + Path.GetExtension(file.FileName);
            string path = Path.Combine(_environment.WebRootPath, "pictures", name);
            using var fileStream = new MemoryStream();
            {
                file.CopyTo(fileStream);
                await System.IO.File.WriteAllBytesAsync(path, fileStream.ToArray());
            }
            product.Image = name;
            product.Category = _productService.GetAllCategories().First(x => x.Id == category);
            await _productService.AddProduct(product);
            return RedirectToAction("Products");
        }


        [AuthorizeFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            return View(product);
        }


        [AuthorizeFilter]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            string path = Path.Combine(_environment.WebRootPath, "pictures", product.Image);
            await _productService.DeleteProduct(id);
            System.IO.File.Delete(path);
            return RedirectToAction("Products");
        }

        [AuthorizeFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _productService.GetAllCategories().ToList();
            return View(product);
        }

        [AuthorizeFilter]
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, Category category, IFormFile file)
        {
            if (file != null)
            {
                string hash = Guid.NewGuid().ToString();
                string name = Path.GetFileNameWithoutExtension(file.FileName) + hash + Path.GetExtension(file.FileName);
                string path = Path.Combine(_environment.WebRootPath, "pictures", name);

                using var fileStream = new MemoryStream();
                {
                    file.CopyTo(fileStream);
                    await System.IO.File.WriteAllBytesAsync(path, fileStream.ToArray());
                }

                if (!string.IsNullOrEmpty(product.Image))
                {
                    string oldImagePath = Path.Combine(_environment.WebRootPath, "pictures", product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                product.Image = name;
            }

            product.Category = _productService.GetAllCategories().First(x => x.Id == category.Id);

            await _productService.UpdateProduct(product);
            return RedirectToAction("Products");
        }
    }
}