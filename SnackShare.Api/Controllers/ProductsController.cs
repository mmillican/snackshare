using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackShare.Api.Data.Entities.Products;
using SnackShare.Api.Mappers;
using SnackShare.Api.Models.Products;
using SnackShare.Api.Services;

namespace SnackShare.Api.Controllers
{
    /// <summary>
    /// Manages products in the system
    /// </summary>
    [Authorize]
    [Route("products")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductsController(IProductService productService,
            ILoggerFactory loggerFactory)
        {
            _productService = productService;
            _logger = loggerFactory.CreateLogger<ProductsController>();
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        /// <response code="200"></response>
        /// <response code="500">Internal error occurred</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productService.Products
                    .OrderBy(x => x.Name)
                    .ProjectTo<ProductModel>()
                    .ToListAsync();

                return Ok(products);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting products");

                return StatusCode(500, "Error getting products");
            }
        }

        /// <summary>
        /// Get a single product by ID
        /// </summary>
        /// <returns>Requested product</returns>
        /// <response code="200">Product found</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal error occurred</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _productService.GetProduct(id);
                if (product == null)
                {
                    _logger.LogInformation($"Product ID {id} not found");
                    return NotFound();
                }

                var model = product.ToModel();

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting product ID {id}");
                return StatusCode(500, "Error getting product");
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="model">Data of product to create</param>
        /// <returns>Newly created product</returns>
        /// <response code="201">Product created</response>
        /// <response code="400">Invalid product data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(ProductModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid data for product");
                return BadRequest(ModelState);
            }

            try
            {
                var product = new Product();
                product.Name = model.Name;
                product.SalePrice = model.SalePrice;
                product.StockQuantity = 0;

                await _productService.CreateProduct(product);
                _logger.LogInformation($"New product ID {product.Id} created");

                model.Id = product.Id;
                
                var newUri = Url.Action(nameof(Get), new { id = model.Id });
                return Created(newUri, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error creating new product, name: {model.Name}");

                return StatusCode(500, "Error creating product");
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">ID of product to update</param>
        /// <param name="model">Product data to update</param>
        /// <returns>Updated product</returns>
        /// <response code="200">Product updated</response>
        /// <response code="400">Bad product data</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid data for product");
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.GetProduct(id);
                if (product == null)
                {
                    _logger.LogInformation($"Product ID {id} not found");
                    return NotFound();
                }

                product.Name = model.Name;
                product.SalePrice = model.SalePrice;

                await _productService.UpdateProduct(product);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product ID {id}");

                return StatusCode(500, "Error updating product");
            }
        }
    }
}
