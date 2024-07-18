using Microsoft.AspNetCore.Mvc;
using Praksa2.Models;
using Praksa2.Services;
using Praksa2.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace Praksa2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productServices;
       // private readonly AppDbContext appDbContext;
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
            var products = await productServices.GetAllProducts();
            return Ok(products);
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
            var product = await productServices.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
       
        public async Task<ActionResult<Products>> Post( Products product)
     {
         if (product == null)
         {
             return BadRequest();
         }

         await productServices.AddProduct(product);
         return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
     }

        [HttpPut]
        public async Task<ActionResult> Put( Products newProduct)
        {
            if (newProduct == null)
            { return BadRequest("Invalid product data."); }
            try
            {
                await productServices.UpdateProduct(newProduct);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating product.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productServices.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            await productServices.DeleteProduct(id);
            return Ok("Success");
        }
    }
}

