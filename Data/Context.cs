using Microsoft.EntityFrameworkCore;
using shop.Models;

namespace shop.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Customer> Customer {get; set;}
        public DbSet<Order> Order {get; set;}
        public DbSet<Product> Product {get; set;}
    }
}