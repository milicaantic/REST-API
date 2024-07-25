using Microsoft.AspNetCore.Mvc;
using Praksa2.Models;
using Praksa2.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Praksa2.Dtos;
using Praksa2.Validators;
using FluentValidation;

using Praksa2.Services;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Dtos;
using DataAccessLayer.Models;
using Microsoft.Extensions.Options;

namespace Praksa2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productServices;
     

        public ProductController(IProductServices productServices)
        {
            this.productServices = productServices;
           
        }
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");

            if (userIdClaim == null)
            {
                throw new Exception("UserID claim not found");
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            else
            {
                throw new Exception("Invalid UserID value");
            }
        }
        private string GetUserName()
        {
            var claims = User.Claims.ToList();
            var usernameClaim = claims.FirstOrDefault(c => c.Type == "Username");

            if (usernameClaim == null)
            {
                throw new Exception("Username claim not found");
            }

            return usernameClaim.Value;
        }

        [HttpGet]
        public IActionResult GetUserProducts()
        {
            try
            {
                var userId = GetUserId();
                var products = productServices.GetUserProducts(userId);
                if (products == null || !products.Any())
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Your products do not exist.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products.");
            }
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            try
            {
                var userId = GetUserId();
                var product = await productServices.GetProductById(id, userId);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving product.");
            }
        }

        [HttpPost]
       
        public async Task<ActionResult<ProductCreateDto>> Post(ProductCreateDto dto)
        {
            var validator = new ProductCreateDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var userId = GetUserId();
            var userName = GetUserName();
            var product = new Products
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                OwnerId = userId,
                CreatedByUser=userName
                
            };
            if (product == null)
            {
                return BadRequest();
            }
            try
            {
                await productServices.AddProduct(product);
                var readDto = new ProductReadDto 
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price
                };
                return CreatedAtAction(nameof(Get), new { id = product.Id }, readDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating product.");
            }
        }
        [HttpPost("assign-product")]
        public IActionResult AssignProductToUser(AssingProductDto dto)
        {
            productServices.AssignProductToUser(dto.UserId, dto.ProductId);
            return Ok("Success");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ProductUpdateDto dto)
        {

            var validator = new ProductUpdateDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var userId = GetUserId();
            var newProduct = new Products
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                OwnerId = userId
            };
            if (newProduct == null)
            {
                return BadRequest();
            }
            if (id != userId)
            {
                return Unauthorized("You do not have permission to update this product.");
            }
            try
            {
                await productServices.UpdateProduct(id, newProduct, userId);
                return Ok("Success");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating product.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                var product = await productServices.GetProductById(id, userId);
                if (product == null)
                {
                    return NotFound();
                }
                if (product.OwnerId != userId)
                {
                    return Unauthorized("You do not have permission to delete this product.");
                }

                await productServices.DeleteProduct(id, userId);
                return Ok("Success");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }

        }
        [HttpGet("info")]
        public async Task<IActionResult> GetProductInfo()
        {
            try
            {
                var totalProductCountTask = productServices.GetTotalProductCountAsync();
                var averagePriceTask = productServices.GetAveragePriceAsync();
                var lowestPriceTask = productServices.GetLowestPriceAsync();
                var highestPriceTask = productServices.GetHighestPriceAsync();
                var totalAssignedProductsCountTask = productServices.GetTotalAssignedProductsCountAsync();


                await Task.WhenAll(totalProductCountTask, averagePriceTask, lowestPriceTask, highestPriceTask, totalAssignedProductsCountTask);


                var totalProductCount = await totalProductCountTask;
                var averagePrice = await averagePriceTask;
                var lowestPrice = await lowestPriceTask;
                var highestPrice = await highestPriceTask;
                var totalAssignedProductCount = await totalAssignedProductsCountTask;


                var response = new ProductInfoResponse
                {
                    TotalProductCount = totalProductCount,
                    AveragePrice = averagePrice,
                    LowestPrice = lowestPrice,
                    HighestPrice = highestPrice,
                    TotalAssignedProductsCount = totalAssignedProductCount
                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Došlo je do greške prilikom preuzimanja informacija o proizvodima.");
            }
        }
        [HttpGet("top-popular")]
        public async Task<IActionResult> GetTopPopularProducts([FromQuery] int? topCount)
        {
            try
            {
              
                var popularProducts = await productServices.GetTopPopularProductsAsync(topCount);
                return Ok(popularProducts);
            }
            catch (Exception ex)
            {
               
                return StatusCode(StatusCodes.Status500InternalServerError, "Došlo je do greške prilikom preuzimanja informacija o najpopularnijim proizvodima.");
            }
        }
    }
}


