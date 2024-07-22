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
        /*public ProductController(AppDbContext appDbContext)
      {

          this.appDbContext = appDbContext;
      }*/
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            try
            {
                var products = await productServices.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
             
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products.");
            }
        }
       /* public ActionResult<List<Products>> Get()
        {
            // Sinhrono dobijanje svih zaposlenih
            var product = appDbContext.Products.ToList();
            return Ok(product);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            try
            {
                var product = await productServices.GetProductById(id);
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
            var product = new Products
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };
            if (product == null)
         {
             return BadRequest();
         }
            try
            {
                await productServices.AddProduct(product);
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
              
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating product.");
            }
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
           
            var newProduct = new Products
            {
                Id=id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };
         
            try
            {
                await productServices.UpdateProduct(id,newProduct);
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
            var product = await productServices.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            
                await productServices.DeleteProduct(id);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }

        }
    }
}

