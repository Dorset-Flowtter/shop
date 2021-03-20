// DEPRECATED

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
    public class CustomersController : ControllerBase
    {
        private readonly Context _context;

        public CustomersController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<customerDTO>>> GetCustomers()
        {
            var customer = from customers in _context.Customer
                           select new customerDTO
                           {
                               Customer_id = customers.id,
                               Customer_firstname = customers.firstname,
                               Customer_lastname = customers.lastname,
                               Customer_username = customers.username,
                               Customer_password = customers.passwordhash,
                           };
            return await customer.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<customerDTO> GetCustomer(int id)
        {
            var customer = from customers in _context.Customer
                           select new customerDTO
                           {
                               Customer_id = customers.id,
                               Customer_firstname = customers.firstname,
                               Customer_lastname = customers.lastname,
                               Customer_username = customers.username,
                               Customer_password = customers.passwordhash,
                           };

            var customer_by_id = customer.ToList().Find(x => x.Customer_id == id);

            if (customer_by_id == null)
            {
                return NotFound();
            }
            return customer_by_id;
        }


        [HttpPost]
        public async Task<ActionResult<customerDTO>> AddCustomer(AddCustomer customerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer()
            {
                firstname = customerDTO.Customer_firstname,
                lastname = customerDTO.Customer_lastname,
                username = customerDTO.Customer_username,
                passwordhash = customerDTO.Customer_password
            };

            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.id }, customerDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = _context.Customer.Find(id);

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, customerDTO order)
        {
            if (id != order.Customer_id || !utils.utils.CustomerExist(_context, id))
            {
                return BadRequest();
            }
            else
            {
                var __customer = _context.Customer.SingleOrDefault(x => x.id == id);

                __customer.id = order.Customer_id;
                __customer.firstname = order.Customer_firstname;
                __customer.lastname = order.Customer_lastname;
                __customer.username = order.Customer_username;
                __customer.passwordhash = order.Customer_password;

                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}