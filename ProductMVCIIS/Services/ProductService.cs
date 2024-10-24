using ProductMVCIIS.Models;

namespace ProductMVCIIS.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8441");
        }

        public async Task<List<Product>> GetProductsAsync(string name = "")
        {
            var response = await _httpClient.GetAsync($"products?name={name}");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            return products;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"products/{product.ID}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
