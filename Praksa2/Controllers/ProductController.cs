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


        [HttpGet("user-products")]
        public IActionResult GetUserProducts()
        {
            try { 
            var userId = GetUserId(); 
            var products = productServices.GetUserProducts(userId);
            return Ok(products);}
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
                var product = await productServices.GetProductById(id,userId);
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
            var product = new Products
            {
                Name = dto.Name, 
                Description = dto.Description,
                Price = dto.Price,
                OwnerId = userId
            };
            if (product == null)
         {
             return BadRequest();
         }
            var readDto = new ProductReadDto
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };
          
            try
            {
                await productServices.AddProduct(product);
                return CreatedAtAction(nameof(Get), new { id = product.Id },readDto);
            }
            catch (Exception ex)
            {
              
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating product.");
            }
        }
        [HttpPost("assign-product")]
        public IActionResult AssignProductToUser([FromBody] AssingProductDto dto)
        {
            productServices.AssignProductToUser(dto.UserId, dto.ProductId);
            return Ok();
        }
            [HttpPut("{id}")]
        public async Task<ActionResult> Put( int id, ProductUpdateDto dto)
        {
            
            var validator = new ProductUpdateDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
         
            try
            {
                var userId = GetUserId();
                var newProduct = new Products
                {
                    Id = id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    OwnerId = userId
                };
                await productServices.UpdateProduct(id,newProduct,userId);
                return Ok("Success");
            }
            catch (InvalidOperationException ex)
            {
               
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating product.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {try
            { 
                var userId = GetUserId();
            var product = await productServices.GetProductById(id,userId);
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
    }
}

