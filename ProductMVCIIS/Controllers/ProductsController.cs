using Microsoft.AspNetCore.Mvc;
using ProductMVCIIS.Models;
using ProductMVCIIS.Services;

namespace ProductMVCIIS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string name = "")
        {
            
            var products = await _productService.GetProductsAsync(name);
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var productsAll = await _productService.GetProductsAsync();
            var productForEdit = productsAll.FirstOrDefault(p => p.ID == id);
            if (productForEdit == null) return NotFound();
            return View(productForEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
