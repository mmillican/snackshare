using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnackShare.Api.Data;
using SnackShare.Api.Data.Entities.Products;

namespace SnackShare.Api.Services
{
    public interface IProductService
    {
        IQueryable<Product> Products { get; }

        Task<Product> GetProduct(int id);
        
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);

        Task<IEnumerable<ProductStock>> GetStockHistory(int productId);
        Task RestockProduct(ProductStock stock);
    }

    public class ProductService : IProductService
    {
        private readonly SnackDbContext _snackDbContext;

        public ProductService(SnackDbContext snackDbContext)
        {
            _snackDbContext = snackDbContext;
        }

        public IQueryable<Product> Products => _snackDbContext.Products;

        public async Task<Product> GetProduct(int id)
        {
            var product = await _snackDbContext.Products.FindAsync(id);
            return product;
        }
        
        public async Task CreateProduct(Product product)
        {
            _snackDbContext.Products.Add(product);
            await _snackDbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _snackDbContext.Products.Remove(product);
            await _snackDbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _snackDbContext.Products.Update(product);
            await _snackDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductStock>> GetStockHistory(int productId)
        {
            var stocks = await _snackDbContext.ProductStock.Where(x => x.ProductId == productId).ToListAsync();
            return stocks;
        }

        public async Task RestockProduct(ProductStock stock)
        {
            if (stock.PurchaseQuantity == 0)
                throw new Exception("PurchaseQuantity cannot be 0");
            if (stock.PurchasePrice == 0)
                throw new Exception("PurchasePrice cannot be 0");

            stock.UnitPrice = CalculateUnitPrice(stock.PurchaseQuantity, stock.PurchasePrice);

            _snackDbContext.ProductStock.Add(stock);

            stock.Product.StockQuantity = stock.Product.StockQuantity + stock.PurchaseQuantity;

            await _snackDbContext.SaveChangesAsync();
        }

        public decimal CalculateUnitPrice(int quantity, decimal totalPrice)
        {
            return totalPrice / quantity;
        }
    }
}
