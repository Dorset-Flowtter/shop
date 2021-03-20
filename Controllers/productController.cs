
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop.Data;
using shop.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shop.Models;
using shop.utils;
using Microsoft.AspNetCore.Authorization;

namespace shop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Context _context;

        public ProductsController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<productDTO>>> GetProducts()
        {
            var product = from products in _context.Product
                          select new productDTO
                          {
                              Product_id = products.id,
                              Product_name = products.name
                          };
            return await product.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<productDTO> GetProduct(int id)
        {
            var product = from products in _context.Product
                          select new productDTO
                          {
                              Product_id = products.id,
                              Product_name = products.name
                          };

            var product_by_id = product.ToList().Find(x => x.Product_id == id);

            if (product_by_id == null)
            {
                return NotFound();
            }
            return product_by_id;
        }


        [HttpPost]
        public async Task<ActionResult<productDTO>> AddProduct(AddProduct productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product()
            {
                name = productDTO.Product_name
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.id }, productDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = _context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
                return product;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, productDTO order)
        {
            if (id != order.Product_id || !utils.utils.ProductExist(_context, id))
            {
                return BadRequest();
            }
            else
            {
                var __product = _context.Product.SingleOrDefault(x => x.id == id);

                __product.id = order.Product_id;
                __product.name = order.Product_name;

                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}