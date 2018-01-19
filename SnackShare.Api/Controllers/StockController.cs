using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnackShare.Api.Data.Entities.Products;
using SnackShare.Api.Mappers;
using SnackShare.Api.Models.Products;
using SnackShare.Api.Services;

namespace SnackShare.Api.Controllers
{
    [Authorize]
    [Route("products/{productId}/stock")]
    public class StockController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public StockController(IProductService productService,
            ILoggerFactory loggerFactory)
        {
            _productService = productService;
            _logger = loggerFactory.CreateLogger<StockController>();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            try
            {
                var stocks = await _productService.GetStockHistory(productId);
                var result = stocks.Select(x => x.ToModel());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting stock history for product ID {productId}");
                return StatusCode(500, "Error getting stock history");
            }
        }

        /// <summary>
        /// Re-stocks a product
        /// </summary>
        /// <param name="productId">Product ID to re-stock</param>
        /// <param name="model">Stock data</param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Create(int productId, [FromBody] ProductStockModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Invalid data submitted for product ID {model.ProductId}");
                return BadRequest(ModelState);
            }

            var product = await _productService.GetProduct(model.ProductId);
            if (product == null)
            {
                _logger.LogInformation($"Product ID {model.ProductId} not found");
                return NotFound();
            }

            try
            {
                var stock = new ProductStock
                {
                    Product = product,
                    PurchaseDate = model.PurchaseDate,
                    PurchaseQuantity = model.PurchaseQuantity,
                    PurchasePrice = model.PurchasePrice,
                    UserId = GetUserId()
                };

                await _productService.RestockProduct(stock);

                return Ok(model); // Not sure if this is what _should_ be returned here or not...
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error re-stocking product ID {model.ProductId}");
                return StatusCode(500, "Error re-stocking product");
            }
        }

        private int GetUserId()
        {
            var identity = User.Identity as ClaimsIdentity;
            int userId;
            if (int.TryParse(identity.FindFirst("ss_user_id")?.Value, out userId))
            {
                return userId;
            }

            return 0;
        }
    }
}