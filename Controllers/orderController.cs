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

// Todo only edit and see your orders

namespace shop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly Context _context;

        public OrdersController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<orderDTO>>> GetOrders()
        {
            var order = from orders in _context.Order
                        select new orderDTO
                        {
                            Order_id = orders.id,
                            Order_customer_id = orders.idcustomer,
                            Order_product_id = orders.idproduct,
                            Order_quantity = orders.quantity
                        };

            return await order.ToListAsync();
        }
        [HttpGet("{id}")]
        public ActionResult<orderDTO> GetOrder(int id)
        {
            var order = from orders in _context.Order
                        select new orderDTO
                        {
                            Order_id = orders.id,
                            Order_customer_id = orders.idcustomer,
                            Order_product_id = orders.idproduct,
                            Order_quantity = orders.quantity
                        };

            var order_by_id = order.ToList().Find(x => x.Order_id == id);

            if (order_by_id == null)
            {
                return NotFound();
            }
            return order_by_id;
        }

        [HttpPost]
        public async Task<ActionResult<orderDTO>> AddOrders(AddOrder orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!utils.utils.CustomerExist(_context, orderDTO.Order_customer_id) || !utils.utils.ProductExist(_context, orderDTO.Order_product_id))
            {
                return BadRequest(ModelState);
            }

            var order = new Order()
            {
                idproduct = orderDTO.Order_product_id,
                idcustomer = orderDTO.Order_customer_id,
                quantity = orderDTO.Order_quantity
            };
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.id }, orderDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var Order = _context.Order.Find(id);

            if (Order == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(Order);
                await _context.SaveChangesAsync();
                return Order;
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, orderDTO order)
        {
            if (id != order.Order_id || !utils.utils.OrderExist(_context, id))
            {
                return BadRequest();
            }
            else
            {
                var __order = _context.Order.SingleOrDefault(x => x.id == id);

                __order.id = order.Order_id;
                __order.idcustomer = order.Order_customer_id;
                __order.idproduct = order.Order_product_id;
                __order.quantity = order.Order_quantity;

                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}